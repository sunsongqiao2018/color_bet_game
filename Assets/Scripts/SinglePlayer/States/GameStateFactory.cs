
public class GameStateFactory
{
    public IdleGameState Idle()
    {
        return new IdleGameState(StateMachine.Instance, this);
    }
    public PlayGameState Play()
    {
        return new PlayGameState(StateMachine.Instance, this);
    }
    public ResultGameState Result()
    {
        return new ResultGameState(StateMachine.Instance, this);
    }
}
