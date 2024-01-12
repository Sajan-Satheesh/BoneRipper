
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GroundEnemyController : EnemyController 
{
    [SerializeField] public GroundEnemyModel enemyModel { get; private set; }

    public GroundEnemyController(GroundEnemyData enemyDetails, Transform spawnTransform) : base(enemyDetails, spawnTransform)
    {
        enemyModel = new GroundEnemyModel(enemyDetails, base.enemyView, spawnTransform.position,this);
        enemyView.getEnemyController(this);
        setAnimationState(EnumEnemyAnimationStates.chasing, true);
        enemyModel.enemy.GetComponent<Rigidbody>().isKinematic = true;
        enemyModel.groundEnemyStateMachine.InitializeState(enemyModel.groundEnemyStateMachine.currentState.state);
    }

    override public void onUpdate()
    {
        enemyModel.groundEnemyStateMachine.currentState.OnTick();
    }

    public void setState(EnumGroundEnemyStates _enemyState)
    {
        enemyModel.groundEnemyStateMachine.SetState(_enemyState);
    }


    public void setAnimationState(
    EnumEnemyAnimationStates state,
    bool blend = false,
    float transitionTime = 1f)
    {
        if (enemyModel.currAnimationState == state) return;

        enemyModel.currAnimationState = state;

        if (!blend)
            enemyModel.enemy.GetComponent<Animator>().Play(enemyModel.currAnimationState.ToString());
        else enemyModel.enemy.GetComponent<Animator>().CrossFade(enemyModel.currAnimationState.ToString(), transitionTime);

    }

    public void move()
    {
        enemyModel.enemy.transform.Translate(enemyModel.enemy.transform.forward * enemyModel.speed * Time.deltaTime, Space.World);
    }

    public void orient(Vector3 toLookPosition)
    {
        Quaternion toLookDirection = Quaternion.LookRotation(base.flatPosDirection(toLookPosition) - base.flatPosDirection(enemyModel.enemy.transform.position), Vector3.up);
        Quaternion finalRotation = Quaternion.Slerp(enemyModel.enemy.transform.rotation, toLookDirection, 0.1f);
        enemyModel.enemy.transform.rotation = finalRotation;
    }
    public List<Transform> getBodyParts()
    {
        return enemyModel.bodyParts;
    }
    public override void destructEnemy()
    {
        base.destructEnemy();
        enemyModel.DestroyModel();
        enemyModel.enemy = null;

    }
    ~GroundEnemyController()
    {
        Debug.Log("destroyed Tower Enemy Controller");
    }

}
