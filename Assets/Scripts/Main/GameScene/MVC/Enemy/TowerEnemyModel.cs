using System;
using System.Threading;
using UnityEngine;

[Serializable]
public class TowerEnemyModel : EnemyModel
{
    public Vector3 projectedTarget = Vector3.zero;
    public Vector3 projectedShootReach = Vector3.zero;
    public Vector3 projectedShootDirection = Vector3.zero;
    public int shootIteration = 0;
    public Coroutine shooting;
    public CancellationTokenSource ct_shooting = null;
    public TowerEnemyState currState;
    internal EnemyAnimationStates currAnimationState;

    public TowerEnemyModel(SO_TowerEnemy enemyDetails, EnemyView enemyView) : base(enemyDetails, enemyView)
    {
        currState = enemyDetails.defaultState;
    }

    public override void destroyModel()
    {
        base.destroyModel();
        shooting = null;
    }

    ~TowerEnemyModel()
    {
        shooting = null;
        Debug.Log("Destroyed TowerEnemy Model");
    }
}
