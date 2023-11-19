using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] PlayerState playerState;
    [SerializeField] PlayerAnimationStates animState;

    public void getPlayerController(PlayerController _playerController)
    {
        playerController= _playerController;
    }


    // Update is called once per frame
    void Update()
    {
        playerController.onUpdate();
        if ((int)Time.time % 2 == 0)
        {
            playerState = playerController.getPlayerState();
            animState = playerController.getAnimState();
        }
           
    }

    public void beginCoroutine(Coroutine coroutine, Func<IEnumerator> func)
    {
        if(coroutine != null) { endCoroutine(coroutine); }
        coroutine = StartCoroutine(func());
    }
    public void endCoroutine(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
        coroutine = null;
    }
}
