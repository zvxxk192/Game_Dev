public interface IGameState
{
    void Enter();
    void Exit();
    void Tick();
}

public abstract class GameBaseState : IGameState
{
    protected GameStateManager ctx;
    public GameBaseState(GameStateManager stateMachine)
    {
        this.ctx = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Tick() { }
}
