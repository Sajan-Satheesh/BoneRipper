using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] Vector3 offsetPosition;
    [SerializeField] offsetConditions conditions;
    // Update is called once per frame
    void Update()
    {

        followObject(PlayerService.instance.getPlayerLocation(), conditions: conditions, offset: offsetPosition);
            
    }

    private void followObject(Vector3 toFollow, Vector3 offset = default, offsetConditions conditions = offsetConditions.full)
    {
        if(toFollow == default) { return; }
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
