public abstract class StateGeneric<EnumState, Controller>
{
    public EnumState state;
    protected Controller controller;
    public bool updateState;
    public StateGeneric(Controller _controller, EnumState _state)
    {
       controller  = _controller;
       state = _state;
        updateState = false;
    }
    public abstract void OnCollision();
    public virtual void OnTick(){
        if (updateState)
        {
            TickableFunction();
        }
    }
    protected abstract void TickableFunction();
    public abstract void OnStateEnter();
    public abstract void OnStateExit();
}