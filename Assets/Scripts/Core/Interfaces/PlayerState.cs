using UnityEngine;

public interface IPlayerState
{
    void Enter();
    void Exit();
    void Tick();
    void FixedTick();
    void HandleInput(PlayerCommand command);
}

public abstract class PlayerBaseState : IPlayerState
{
    protected PlayerStateMachine ctx;
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.ctx = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Tick() { }
    public virtual void FixedTick() { }
    public virtual void HandleInput(PlayerCommand command) { }
}