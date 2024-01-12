using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkOutsideGroundEnemy : GroundEnemyState
{
    public WalkOutsideGroundEnemy(GroundEnemyController _controller, EnumGroundEnemyStates _state) : base(_controller, _state) { }

    public override void OnCollision()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateEnter()
    {
        controller.enemyModel.enemy.transform.forward = (EnemyService.instance.requestLandCenter() - controller.enemyModel.enemy.transform.position).normalized;
    }

    public override void OnStateExit()
    {
        controller.enemyModel.enemy.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    protected override void TickableFunction()
    {
        controller.enemyModel.enemy.transform.forward = (EnemyService.instance.requestLandCenter() - controller.enemyModel.enemy.transform.position).normalized;
        controller.move();
        controller.orient(controller.enemyModel.enemy.transform.forward);
        if (Vector3.Distance(controller.enemyModel.enemy.transform.position, controller.enemyModel.spawnPostion) > 3)
        {
            controller.setState(EnumGroundEnemyStates.CHASING);
        }
    }
}
