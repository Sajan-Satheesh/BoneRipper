
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

[Serializable]
public class TowerEnemyController : EnemyController 
{
    [SerializeField] public TowerEnemyModel enemyModel { get; private set; }

    public TowerEnemyController(TowerEnemyData towerEnemyDetails, Vector3 spawnPosition) : base(towerEnemyDetails, spawnPosition)
    {
        enemyModel = new TowerEnemyModel(towerEnemyDetails, enemyView, this);
        enemyModel.towerEnemyStateMachine.InitializeState(enemyModel.towerEnemyStateMachine.currentState.state);
    }

    override public void onUpdate()
    {
        enemyModel.towerEnemyStateMachine.currentState.OnTick();
    }
    public List<Transform> getBodyParts()
    {
        return enemyModel.bodyParts;
    }

    public void setAnimationState(
    EnumEnemyAnimationStates state,
    bool blend = false,
    float transitionTime = 1f)
    {
        if (enemyModel.currAnimationState == state) return;

        enemyModel.currAnimationState = state;

        if (!blend)
            enemyModel.enemy.GetComponent<Animator>().Play(state.ToString());
        else enemyModel.enemy.GetComponent<Animator>().CrossFade(state.ToString(), transitionTime);

    }
    public void setState(EnumTowerEnemyStates _enemyState)
    {
        enemyModel.towerEnemyStateMachine.SetState(_enemyState);
        /*if (enemyModel.currState == _enemyState) return;
        prepareState(_enemyState);
        enemyModel.currState= _enemyState;*/
    }

    public void turnTowardsTarget()
    {
        orient(EnemyService.instance.requestPlayerLocation());
    }

    void orient(Vector3 toLookPosition)
    {
        Quaternion toLookDirection = Quaternion.LookRotation(base.flatPosDirection(toLookPosition) - base.flatPosDirection(enemyModel.enemy.transform.position), Vector3.up);
        Quaternion finalRotation = Quaternion.Slerp(enemyModel.enemy.transform.rotation, toLookDirection, 0.1f);
        enemyModel.enemy.transform.rotation = finalRotation;
    }

    Vector3 getEnemyLoc()
    {
        return enemyModel.enemy.transform.position;
    }

    public override void destructEnemy()
    {
        base.destructEnemy();
        enemyModel.DestroyModel();
        enemyModel.enemy = null;

    }
    ~TowerEnemyController()
    {
        Debug.Log("destroyed Tower Enemy Controller");
    }

}
