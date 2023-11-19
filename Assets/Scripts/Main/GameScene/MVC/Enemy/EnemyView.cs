using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    EnemyController enemyController;

    public void getEnemyController(EnemyController _enemyController)
    {
        enemyController = _enemyController;
    }


    // Update is called once per frame
    void Update()
    {
        enemyController.onUpdate();
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
