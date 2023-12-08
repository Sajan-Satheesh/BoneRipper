
using System;
using UnityEngine;

[Serializable]
public class EnemyController : Controller
{
    [SerializeField] protected EnemyView enemyView;

    public EnemyController(SO_Enemy enemyDetails, Transform spawnTransform)
    {
        enemyView = GameObject.Instantiate<EnemyView>(enemyDetails.enemyView, spawnTransform.position, spawnTransform.rotation, EnemyService.instance.enemyRoot);
        enemyView.getEnemyController(this);
        enemyView.transform.forward = spawnTransform.forward;
    }
    public EnemyController(SO_Enemy enemyDetails, Vector3 spawnPosition)
    {
        enemyView = GameObject.Instantiate<EnemyView>(enemyDetails.enemyView, spawnPosition, Quaternion.identity, EnemyService.instance.enemyRoot);
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



