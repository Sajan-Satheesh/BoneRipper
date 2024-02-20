
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class EnemyController : Controller
{
    protected EnemyView enemyView;
    //protected EnemyModel enemyModel;

    public EnemyController(EnemyData enemyDetails, Transform spawnTransform)
    {
        enemyView = Object.Instantiate<EnemyView>(enemyDetails.enemyView, spawnTransform.position, spawnTransform.localRotation, EnemyService.instance.enemyRoot);
        enemyView.getEnemyController(this);
    }
    public EnemyController(EnemyData enemyDetails, Vector3 spawnPosition)
    {
        enemyView = Object.Instantiate<EnemyView>(enemyDetails.enemyView, spawnPosition, Quaternion.identity, EnemyService.instance.enemyRoot);
        enemyView.getEnemyController(this);

    }



    public void setEnemyRoot(Transform parent)
    {
        enemyView.gameObject.transform.parent = parent;
    }

    virtual public void onUpdate() { }

    protected Vector3 flatPosDirection(Vector3 dir)
    {
        return new Vector3(dir.x, 0f, dir.z);
    }

    virtual public void destructEnemy()
    {
        if (enemyView != null) UnityEngine.Object.Destroy(enemyView.gameObject);
    }

    ~EnemyController()
    {
        if(enemyView!=null) UnityEngine.Object.Destroy(enemyView.gameObject);

    }

}



