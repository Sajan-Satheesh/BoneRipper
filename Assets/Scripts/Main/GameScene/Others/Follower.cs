using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] Transform offsetObject;
    [SerializeField] Vector3 offsetPosition;
    [SerializeField] offsetConditions conditions;

    private void Awake()
    {
        if (offsetObject != null) { offsetPosition = offsetObject.position; }
    }
    // Update is called once per frame
    void Update()
    {

        followObject(PlayerService.instance.getPlayerLocation(), conditions: conditions, offset: offsetPosition);
            
    }

    private void followObject(Vector3 toFollow, Vector3 offset = default, offsetConditions conditions = offsetConditions.full)
    {
        switch (conditions)
        {
            case offsetConditions.full:
                transform.position = toFollow + offsetPosition;
                break;
            case offsetConditions.xzPlane:
                transform.position = new Vector3(toFollow.x, transform.position.y, toFollow.z);
                break;
            default:
                break;
        }
    }

    enum offsetConditions { full, xzPlane  }

}
