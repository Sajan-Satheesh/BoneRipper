using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour, IDestructable
{
    [SerializeField] PlayerController playerController;

    public void getPlayerController(PlayerController _playerController)
    {
        playerController= _playerController;
    }


    // Update is called once per frame
    void Update()
    {
        playerController.onUpdate();     
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
            playerController.requestGameOver();
        }
    }

    public Controller getController()
    {
        return playerController;
    }

    Destructables IDestructable.getDestructableType()
    {
        return Destructables.player;
    }
}
