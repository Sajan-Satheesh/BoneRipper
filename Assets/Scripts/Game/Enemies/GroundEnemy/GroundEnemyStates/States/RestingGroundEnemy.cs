using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingGroundEnemy : GroundEnemyState
{
    public RestingGroundEnemy(GroundEnemyController _controller, EnumGroundEnemyStates _state) : base(_controller, _state)
    {
    }

    public override void OnCollision()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateEnter()
    {
        controller.setAnimationState(EnumEnemyAnimationStates.resting, true);
    }

    public override void OnStateExit()
    {
        //throw new System.NotImplementedException();
    }

    protected override void TickableFunction()
    {
        //throw new System.NotImplementedException();
    }
}
