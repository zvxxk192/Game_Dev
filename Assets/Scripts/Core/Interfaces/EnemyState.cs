using UnityEngine;

public interface IEnemyState
{
    void Enter();
    void Exit();
    void Tick();
}

public abstract class EnemyBaseState : IEnemyState
{
    protected EnemyStateMachine ctx;
    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.ctx = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Tick() { }
}
