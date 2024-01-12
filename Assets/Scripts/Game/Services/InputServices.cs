using UnityEngine;

public class InputServices : GenericSingleton<InputServices>
{
    [SerializeField] GameObject touchCollider;
    public LayerMask directionHit;
    RaycastHit mouseHitOnWorld;
    Vector3 mousePosOnScreen;
    Vector3 mousePosOnWorld;

    private void Start()
    {
        Instantiate(touchCollider);
    }
    public Vector3 GetMousePostion()
    {
        mousePosOnScreen = Input.mousePosition;
        return mousePosOnScreen;
    }

    public Vector3 GetMousePosOnWorld()
    {
        Ray CamToWorld = Camera.main.ScreenPointToRay(GetMousePostion());
        Physics.Raycast(CamToWorld, out mouseHitOnWorld, float.MaxValue, directionHit);
        mousePosOnWorld = GetPosOnHeight(mouseHitOnWorld.point, 0f);
        Debug.DrawRay(Camera.main.transform.position, mousePosOnWorld - Camera.main.transform.position, Color.red);
        return mousePosOnWorld;
    }
    private Vector3 GetPosOnHeight(Vector3 pos, float height)
    {
        return new Vector3(pos.x, height, pos.z);
    }
}
