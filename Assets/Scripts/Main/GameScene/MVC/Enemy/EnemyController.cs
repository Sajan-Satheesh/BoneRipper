
using System;
using UnityEngine;

[Serializable]
public class EnemyController 
{
    [SerializeField] EnemyModel enemyModel;
    [SerializeField] EnemyView enemyView;

    public EnemyController(SO_Enemy enemyDetails, Vector3 spawnPosition)
    {
        enemyView = GameObject.Instantiate<EnemyView>(enemyDetails.enemyView, spawnPosition, Quaternion.identity, EnemyService.instance.enemyRoot);
        enemyView.getEnemyController(this);
        enemyModel = new EnemyModel(enemyDetails, enemyView);

    }

    public void onUpdate()
    {
        runStateMachine();
    }

    public void setState(EnemyState _enemyState)
    {
        enemyModel.currState= _enemyState;
    }
    private void runStateMachine()
    {
        switch (enemyModel.currState)
        {
            case EnemyState.load:
                break;
            case EnemyState.chasing:
                chase();
                break;
            case EnemyState.attacking:
                break;
            case EnemyState.resting:
                break;
            case EnemyState.walkingToInn:
                break;
            default:
                break;
        }
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

    void orient(Vector3 playerPos)
    {
        Quaternion toLookDirection = Quaternion.LookRotation(flatPosDirection(playerPos) - flatPosDirection(enemyModel.enemy.transform.position), Vector3.up);
        Quaternion finalRotation = Quaternion.Slerp(enemyModel.enemy.transform.rotation, toLookDirection, 0.1f);
        enemyModel.enemy.transform.rotation = finalRotation;
    }

    Vector3 flatPosDirection(Vector3 dir)
    {
        return new Vector3(dir.x, 0f, dir.z);
    }

}
