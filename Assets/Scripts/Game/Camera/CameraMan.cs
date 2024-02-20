using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    [SerializeField] float distanceFromTarget;
    // Update is called once per frame
    void LateUpdate()
    {
        if(PlayerService.instance.isPlayerAlive())
            transform.position = PlayerService.instance.getPlayerLocation() + transform.forward * -distanceFromTarget;
    }
}
