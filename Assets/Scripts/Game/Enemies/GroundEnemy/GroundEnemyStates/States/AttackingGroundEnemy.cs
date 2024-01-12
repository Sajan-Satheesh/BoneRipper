using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingGroundEnemy : GroundEnemyState
{
    public AttackingGroundEnemy(GroundEnemyController _controller, EnumGroundEnemyStates _state) : base(_controller, _state) { }

    public override void OnCollision()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateEnter()
    {
        controller.setAnimationState(EnumEnemyAnimationStates.attacking, true , 0.5f);
    }

    public override void OnStateExit()
    {
        //throw new System.NotImplementedException();
    }

    protected override void TickableFunction()
    {
        attacking();
    }
    private void attacking()
    {
        //Debug.Log($"animation normalised time : {controller.enemyModel.enemy.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime}");
        //Debug.Log($"animatior state info length : {controller.enemyModel.enemy.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length}");
        controller.orient(PlayerService.instance.getPlayerLocation());
        if (controller.enemyModel.enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfoCount(0) == 1)
        {
            controller.setState(EnumGroundEnemyStates.CHASING);
        }
    }
}
