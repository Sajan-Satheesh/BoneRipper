using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AttackingTowerEnemy : TowerEnemyState
{
    private float playerSpeed = 0f;
    private float normArrowSpeed = 0f;
    private Vector3 projectedTarget = Vector3.zero;
    private Vector3 projectedShootReach = Vector3.zero;
    private Vector3 projectedShootDirection = Vector3.zero;
    private int shootIteration = 0;

    public AttackingTowerEnemy(TowerEnemyController _controller, EnumTowerEnemyStates _state) : base(_controller, _state)
    {
    }

    public override void OnCollision()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateEnter()
    {
        playerSpeed = EnemyService.instance.requestPlayerSpeed();
        normArrowSpeed = 10 / playerSpeed;
        if (normArrowSpeed <= 1)
        {
            Debug.LogWarning("trying to shoot inefficient Arrow");
            return;
        }
        resetProjections();
        shootAtTarget();
    }

    public override void OnStateExit()
    {
        //throw new System.NotImplementedException();
    }

    protected override void TickableFunction()
    {
        controller.turnTowardsTarget();
    }

    private async void shootAtTarget()
    {
        controller.enemyModel.ct_shooting = new CancellationTokenSource();
        float playerSpeed = EnemyService.instance.requestPlayerSpeed();
        Vector3 playerPos = EnemyService.instance.requestPlayerLocation();
        Vector3 playerDirection = EnemyService.instance.requestPlayerDirection();
        float normArrowSpeed = 10 / playerSpeed;
        while (!canArrowReachPlayer())
        {
            calculateTarget(playerPos, playerDirection, normArrowSpeed);
            ++shootIteration;
            await Task.Yield();
        }
        if (Physics.Raycast(controller.enemyModel.enemy.transform.position, projectedShootDirection, Vector3.Distance(projectedTarget, controller.enemyModel.enemy.transform.position) - 0.2f, EnemyService.instance.sightHiddingLayer))
        {
            Debug.DrawLine(controller.enemyModel.enemy.transform.position, controller.enemyModel.enemy.transform.position + projectedShootDirection * (Vector3.Distance(projectedTarget, controller.enemyModel.enemy.transform.position) - 0.2f), Color.red, 5f);
            return;
        }
        controller.setAnimationState(EnumEnemyAnimationStates.throwing, true);
        releaseArrow();
    }
    private void resetProjections()
    {
        shootIteration = 1;
        projectedTarget = Vector3.zero;
        projectedShootReach = Vector3.zero;
        projectedShootDirection = Vector3.zero;
    }

    bool canArrowReachPlayer()
    {
        float dist_shooter_target = Vector3.Distance(projectedTarget, controller.enemyModel.enemy.transform.position);
        float dist_shooter_reach = Vector3.Distance(projectedShootReach, controller.enemyModel.enemy.transform.position);
        return dist_shooter_reach > dist_shooter_target;
    }

    private void calculateTarget(Vector3 playerPos, Vector3 playerDirection, float normArrowSpeed)
    {

        calculateProjectedTarget(playerPos, playerDirection);
        calculateprojetedShoot(normArrowSpeed);
    }
    private void calculateProjectedTarget(Vector3 targetPos, Vector3 targetDirection)
    {
        targetDirection = targetDirection.normalized;
        projectedTarget = targetPos + targetDirection * shootIteration;
    }

    private void calculateprojetedShoot(float arrowSpeed)
    {
        projectedShootDirection = (projectedTarget - controller.enemyModel.releaseHand.position).normalized;
        projectedShootReach = (controller.enemyModel.releaseHand.position + projectedShootDirection * arrowSpeed * shootIteration);
    }
    private void releaseArrow()
    {
        WeaponServices.instance.ShootArrows(controller.enemyModel.releaseHand.position, projectedShootDirection);
        NextState();
    }

    private void NextState()
    {
        controller.setState(EnumTowerEnemyStates.RESTING);
    }
}
