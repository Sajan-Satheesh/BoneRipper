using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour, IDestructable
{
    [SerializeField] PlayerController playerController = null;

    public void getPlayerController(PlayerController _playerController)
    {
        playerController= _playerController;
    }


    // Update is called once per frame
    void Update()
    {
        playerController?.OnUpdate();     
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 4)
        {
            playerController.RequestGameOver();
        }
    }

    public Controller GetController()
    {
        return playerController;
    }

    EnumDestructables IDestructable.GetDestructableType()
    {
        return EnumDestructables.PLAYER;
    }
}
