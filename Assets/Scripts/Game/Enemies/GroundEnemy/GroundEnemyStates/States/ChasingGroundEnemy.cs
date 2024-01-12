using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingGroundEnemy : GroundEnemyState
{
    public ChasingGroundEnemy(GroundEnemyController _controller, EnumGroundEnemyStates _state) : base(_controller, _state)
    {
    }

    public override void OnCollision()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateEnter()
    {
        controller.setAnimationState(EnumEnemyAnimationStates.chasing, true, 0.5f);
    }

    public override void OnStateExit()
    {
        //throw new System.NotImplementedException();
    }

    protected override void TickableFunction()
    {
        chase();
        SwitchStateConditions();
    }
    private void chase()
    {
        controller.move();
        controller.orient(PlayerService.instance.getPlayerLocation());
    }
    private void SwitchStateConditions()
    {
        if (Vector3.Distance(controller.enemyModel.enemy.transform.position, EnemyService.instance.requestPlayerLocation()) < EnemyService.instance.groundEnemyMinAttackRadius)
        {
            controller.setState(EnumGroundEnemyStates.ATTACKING);
        }
    }
}
