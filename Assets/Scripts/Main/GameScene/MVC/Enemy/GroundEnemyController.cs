
using System;
using UnityEngine;

[Serializable]
public class GroundEnemyController : EnemyController 
{
    [SerializeField] GroundEnemyModel enemyModel;

    public GroundEnemyController(SO_GroundEnemy enemyDetails, Transform spawnTransform) : base(enemyDetails, spawnTransform)
    {
        enemyModel = new GroundEnemyModel(enemyDetails, base.enemyView, spawnTransform.position);
        enemyView.getEnemyController(this);
    }

    override public void onUpdate()
    {
        runStateMachine();
    }

    public void setState(GroundEnemyState _enemyState)
    {
        enemyModel.currState= _enemyState;
    }
    private void runStateMachine()
    {
        switch (enemyModel.currState)
        {
            case GroundEnemyState.load:
                break;
            case GroundEnemyState.chasing:
                chase();
                break;
            case GroundEnemyState.attacking:
                break;
            case GroundEnemyState.resting:
                break;
            case GroundEnemyState.walkFromInn:
                walkOutside();
                break;
            default:
                break;
        }
    }

    private void walkOutside()
    {
        move();
        orient(enemyModel.enemy.transform.forward);
        if (Vector3.Distance(enemyModel.enemy.transform.position, enemyModel.spawnPostion) > 3) setState(GroundEnemyState.chasing);
    }

    private void chase()
    {
        move();
        orient(PlayerService.instance.getPlayerLocation());
    }

    void move()
    {
        enemyModel.enemy.transform.Translate(enemyModel.enemy.transform.forward * enemyModel.speed * Time.deltaTime, Space.World);
    }

    void orient(Vector3 toLookPosition)
    {
        Quaternion toLookDirection = Quaternion.LookRotation(base.flatPosDirection(toLookPosition) - base.flatPosDirection(enemyModel.enemy.transform.position), Vector3.up);
        Quaternion finalRotation = Quaternion.Slerp(enemyModel.enemy.transform.rotation, toLookDirection, 0.1f);
        enemyModel.enemy.transform.rotation = finalRotation;
    }
    public override void destructEnemy()
    {
        base.destructEnemy();
        enemyModel.destroyModel();
        enemyModel.enemy = null;

    }
    ~GroundEnemyController()
    {
        Debug.Log("destroyed Tower Enemy Controller");
    }

}
