public class TowerEnemyStateMachine : StateMachineGeneric<TowerEnemyState, TowerEnemyController, EnumTowerEnemyStates>
{
    public TowerEnemyStateMachine(TowerEnemyController _controller, EnumTowerEnemyStates defaultState) : base(_controller, defaultState) { }

    public override TowerEnemyState GetState(EnumTowerEnemyStates stateEnum)
    {
        int index = CheckWithAvailableStates(stateEnum);
        if (index >= 0) { return states[index]; }

        TowerEnemyState towerEnemyState = null;
        switch (stateEnum)
        {
            case EnumTowerEnemyStates.ATTACKING:
                towerEnemyState = new AttackingTowerEnemy(controller, EnumTowerEnemyStates.ATTACKING);
                break;
            case EnumTowerEnemyStates.RESTING:
                towerEnemyState = new RestingTowerEnemy(controller, EnumTowerEnemyStates.RESTING);
                break;
            default:
                break;
        }
        states.Add(towerEnemyState);
        return towerEnemyState;
    }

    public override void InitializeState(EnumTowerEnemyStates stateEnum)
    {
        currentState = GetState(stateEnum);
        currentState.OnStateEnter();
        currentState.updateState = true;
    }

    public override void SetState(EnumTowerEnemyStates stateEnum)
    {
        currentState.updateState = false;
        currentState.OnStateExit();
        currentState = GetState(stateEnum);
        currentState.OnStateEnter();
        currentState.updateState = true;
    }
    private int CheckWithAvailableStates(EnumTowerEnemyStates stateEnum)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].state == stateEnum) return i;
        }
        return -1;
    }
}
