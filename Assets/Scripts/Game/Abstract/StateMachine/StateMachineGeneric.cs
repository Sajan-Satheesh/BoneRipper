using System.Collections.Generic;

[System.Serializable]
public abstract class StateMachineGeneric<State,Controller,EnumState>
{
	protected Controller controller;
	public State currentState { get; protected set; }
	protected List<State> states = new();

	public StateMachineGeneric(Controller _controller, EnumState defaultState)
	{
		controller = _controller;
        currentState = GetState(defaultState);
    }

	public abstract void InitializeState(EnumState stateEnum);
	public abstract void SetState(EnumState stateEnum);
    public abstract State GetState(EnumState stateEnum);
}
