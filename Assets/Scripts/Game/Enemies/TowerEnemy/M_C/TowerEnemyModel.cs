using System;
using System.Threading;
using UnityEngine;

[Serializable]
public class TowerEnemyModel : EnemyModel
{
    public CancellationTokenSource ct_shooting = null;
    public TowerEnemyStateMachine towerEnemyStateMachine= null; 

    public TowerEnemyModel(TowerEnemyData enemyDetails, EnemyView enemyView, TowerEnemyController controller) : base(enemyDetails, enemyView)
    {
        towerEnemyStateMachine = new TowerEnemyStateMachine(controller, enemyDetails.defaultState);
    }

    public override void DestroyModel()
    {
        base.DestroyModel();
        ct_shooting = null;
    }

    ~TowerEnemyModel()
    {
        ct_shooting = null;
        Debug.Log("Destroyed TowerEnemy Model");
    }
}
