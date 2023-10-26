using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mock_Player : MonoBehaviour
{
    [SerializeField] private Vector2 mousePosition;
    [SerializeField] RaycastHit positionToMove;
    [SerializeField] private float speed;
    //[SerializeField] GameObject mousePositionObject;
    [SerializeField] Camera SceneCamera;
    [SerializeField] float raySpeed;
    [SerializeField] float rayLength;
    private float offset = 0f;
    [SerializeField] float distancefrimCenter;
    [SerializeField] bool hitterRay;
    Ray finder ;
    [SerializeField] LayerMask destructible;
    [SerializeField] LayerMask directionDecision;
    [SerializeField] public bool playable = false;
    [SerializeField] public bool runnable = false;
    [SerializeField] MeshGenerator meshgenerator;
    [SerializeField] private QuadraticCurve qCurve;
    [SerializeField] private BoatMock boat;

    Coroutine enemyDetection;
    Coroutine jumping;

    private void Awake()
    {
        SceneCamera = Camera.main;
    }
    private void Start()
    {
        finder = new(gameObject.transform.position, gameObject.transform.forward);
        enemyDetection = StartCoroutine(changeRaySpeed());
    }

    void Update()
    {
        
        if(playable)
        {
            move();
           
        }
        if(runnable)
        {
            orient();
            createHitterRay();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            Vector3 positionAtYzero = new Vector3(transform.position.x, Random.Range(0,3), transform.position.z);
            meshgenerator.generateMesh(positionAtYzero + transform.forward * meshgenerator.radius * 2);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpToIsland(qCurve.recordPath());
        }

    }

    void move()
    {
        gameObject.transform.Translate(gameObject.transform.forward * speed * Time.deltaTime,Space.World);
    }

    void orient()
    {
        Vector3 toLookForwardVector = updateMousePosition() - new Vector3(gameObject.transform.position.x, 0f, gameObject.transform.position.z);
        Quaternion toLookDirection = Quaternion.LookRotation(toLookForwardVector, Vector3.up);
        Quaternion finalRotation = Quaternion.Slerp(gameObject.transform.rotation, toLookDirection, 0.1f);
        gameObject.transform.rotation = finalRotation;
    }

    Vector3 updateMousePosition()
    {
        mousePosition = Input.mousePosition;
        Ray CamToWorld = SceneCamera.ScreenPointToRay(mousePosition);
        Physics.Raycast(CamToWorld, out positionToMove, float.MaxValue, directionDecision);
        Vector3 mouseInWorldPosition = new Vector3(positionToMove.point.x, 0f, positionToMove.point.z);
        Debug.DrawRay(SceneCamera.transform.position, mouseInWorldPosition - SceneCamera.transform.position, Color.red);
        return mouseInWorldPosition;
    }

    void createHitterRay()
    {
        destructionRay(finder);
        finder.origin = moveObjectLandR(gameObject.transform.forward);
        finder.direction = gameObject.transform.forward;
        
    }

    Vector3 moveObjectLandR(Vector3 mainObjPosition)
    {
        return gameObject.transform.position + gameObject.transform.right * offset;
    }

    IEnumerator changeRaySpeed()
    {
        while (true)
        {
            if (hitterRay)
            {
                if (Vector3.Distance(finder.origin, gameObject.transform.position) > distancefrimCenter)
                {
                    raySpeed *= -1;
                }
                offset += raySpeed;
                Debug.DrawRay(finder.origin, finder.direction.normalized * rayLength, Color.red);
            }
            yield return new WaitForSecondsRealtime(0.001f);
        }
        
    }

    void destructionRay(Ray hitter)
    {
        RaycastHit hitOut;
        if (Physics.Raycast(hitter, out hitOut, rayLength, destructible))
        {
            Destroy(hitOut.collider.gameObject);
        };
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void jumpToIsland(List<Vector3> motionPath)
    {
        boat.deOccupy();
        runnable = false;
        playable = false;
        jumping = StartCoroutine(jumpingMotion(motionPath));

    }

    private IEnumerator jumpingMotion(List<Vector3> motionPath)
    {
        float defaultSpeed = 0.05f;
        float speed = defaultSpeed;
        while (motionPath.Count != 1)
        {
            speed += defaultSpeed;
            gameObject.transform.position = Vector3.Lerp(motionPath[0], motionPath[1], speed);
            if (speed >= 1f)
            {
                speed = defaultSpeed;
                motionPath.RemoveAt(0);
            }
            yield return null;
        }
        playable = true;
        runnable = true;
    }
}
