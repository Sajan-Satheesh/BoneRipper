using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mock_Enemy : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] private float speed;
    [SerializeField] float raySpeed;
    [SerializeField] float rayLength;
    private float offset = 0f;
    [SerializeField] float distancefrimCenter;
    [SerializeField] bool hitterRay;
    Ray finder;
    [SerializeField] LayerMask destructible;
    // Start is called before the first frame update
    void Start()
    {
        finder = new(gameObject.transform.position, gameObject.transform.forward);
        StartCoroutine(changeRaySpeed());
    }

    // Update is called once per frame
    void Update()
    {
        move();
        orient();
        createHitterRay();
    }

    void move()
    {
        gameObject.transform.Translate(gameObject.transform.forward * speed * Time.deltaTime, Space.World);
    }

    Vector3 YawDirection(Vector3 dir)
    {
        return new Vector3(dir.x ,0f, dir.z);
    }

    void orient()
    {
        if(player == null) { return; }
        Quaternion toLookDirection = Quaternion.LookRotation(YawDirection(player.transform.position) - YawDirection(transform.position), Vector3.up);
        Quaternion finalRotation = Quaternion.Slerp(gameObject.transform.rotation, toLookDirection, 0.1f);
        gameObject.transform.rotation = finalRotation;
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
        while (hitterRay)
        {
            if (Vector3.Distance(finder.origin, gameObject.transform.position) > distancefrimCenter)
            {
                raySpeed *= -1;
            }
            offset += raySpeed;
            Debug.DrawRay(finder.origin, finder.direction.normalized * rayLength, Color.red);
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
}
