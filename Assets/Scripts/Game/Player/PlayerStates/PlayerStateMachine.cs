public class PlayerStateMachine :  StateMachineGeneric<PlayerState,PlayerController,EnumPlayerStates>
{
    public PlayerStateMachine(PlayerController _controller, EnumPlayerStates defaultState) : base(_controller, defaultState) { }

    public override PlayerState GetState(EnumPlayerStates stateEnum)
    {
        //if (currentState.state == stateEnum) return currentState;

        int index = checkWithAvailableStates(stateEnum);
        if (index >= 0) { return states[index]; }

        PlayerState playerState = null;
        switch (stateEnum)
        {
            case EnumPlayerStates.RUNNING:
                playerState = new RuningPlayerState(controller);
                break;
            case EnumPlayerStates.DEAD:
                break;
            case EnumPlayerStates.JUMPING:
                playerState = new JumpingPlayerState(controller);
                break;
            case EnumPlayerStates.IN_BOAT:
                playerState = new InBoatPlayerState(controller);
                break;
            case EnumPlayerStates.LEVEL_END:
                playerState = new ExitingIslandPlayerState(controller);
                break;
            default:
                break;
        }
        states.Add(playerState);
        return playerState;

    }

    private int checkWithAvailableStates(EnumPlayerStates stateEnum)
    {
        for(int i=0; i<states.Count; i++)
        {
            if (states[i].state == stateEnum) return i;
        }
        return -1;
    }

    public override void SetState(EnumPlayerStates stateEnum)
    {
        currentState.updateState = false;
        currentState.OnStateExit();
        currentState = GetState(stateEnum);
        currentState.OnStateEnter();
        currentState.updateState = true;
    }

    public override void InitializeState(EnumPlayerStates stateEnum)
    {
        currentState = GetState(stateEnum);
        currentState.OnStateEnter();
        currentState.updateState = true;
    }
}
