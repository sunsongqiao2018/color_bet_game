

/// <summary>
/// abstract class for game states.
/// </summary>
public abstract class BaseGameState
{
    protected StateMachine sMachine;
    protected GameStateFactory sFactory;

    public BaseGameState(StateMachine machine, GameStateFactory factory)
    {
        sMachine = machine;
        sFactory = factory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

    public virtual void UpdateStates() { }

    public virtual void SwitchStates(BaseGameState newState)
    {
        //Out from current State;
        ExitState();

        newState.EnterState();
        sMachine._currentState = newState;
    }
}
