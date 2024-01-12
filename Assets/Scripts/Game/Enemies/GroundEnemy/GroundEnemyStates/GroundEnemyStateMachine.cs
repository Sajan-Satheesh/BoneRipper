using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyStateMachine : StateMachineGeneric<GroundEnemyState, GroundEnemyController, EnumGroundEnemyStates>
{
    public GroundEnemyStateMachine(GroundEnemyController _controller, EnumGroundEnemyStates defaultState) : base(_controller, defaultState) { }

    public override GroundEnemyState GetState(EnumGroundEnemyStates stateEnum)
    {
        int index = CheckWithAvailableStates(stateEnum);
        if (index >= 0) { return states[index]; }

        GroundEnemyState groundEnemyState = null;
        switch (stateEnum)
        {
            case EnumGroundEnemyStates.WALK_OUTSIDE:
                groundEnemyState = new WalkOutsideGroundEnemy(controller,EnumGroundEnemyStates.WALK_OUTSIDE);
                break;
            case EnumGroundEnemyStates.ATTACKING:
                groundEnemyState = new AttackingGroundEnemy(controller, EnumGroundEnemyStates.ATTACKING);
                break;
            case EnumGroundEnemyStates.CHASING:
                groundEnemyState = new ChasingGroundEnemy(controller, EnumGroundEnemyStates.CHASING);
                break;
            case EnumGroundEnemyStates.RESTING:
                groundEnemyState = new RestingGroundEnemy(controller, EnumGroundEnemyStates.RESTING);
                break;
            default:
                break;
        }
        states.Add(groundEnemyState);
        return groundEnemyState;
    }

    public override void InitializeState(EnumGroundEnemyStates stateEnum)
    {
        currentState = GetState(stateEnum);
        currentState.OnStateEnter();
        currentState.updateState = true;
    }

    public override void SetState(EnumGroundEnemyStates stateEnum)
    {
        currentState.updateState = false;
        currentState.OnStateExit();
        currentState = GetState(stateEnum);
        currentState.OnStateEnter();
        currentState.updateState = true;
    }

    private int CheckWithAvailableStates(EnumGroundEnemyStates stateEnum)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].state == stateEnum) return i;
        }
        return -1;
    }

}
