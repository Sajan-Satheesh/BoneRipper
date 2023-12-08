using UnityEngine;

public class HideOutView : MonoBehaviour
{
    public Vector3 TopEnemySpwanLoc;

    private void Awake()
    {
        TopEnemySpwanLoc = transform.GetChild(0).position;
    }

}
