public interface IGameState
{
    void Enter();
    void Exit();
    void Tick();
}

public abstract class GameBaseState : IGameState
{
    protected GameContext ctx;
    public GameBaseState(GameContext gameContext)
    {
        this.ctx = gameContext;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Tick() { }
}
