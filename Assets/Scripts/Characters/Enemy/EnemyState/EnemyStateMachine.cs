using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [Header("Data Source")]
    public EnemyData Data;

    public IEnemyState CurrentState { get; private set; }

    public EnemyController EnemyController { get; private set; }
    public EnemyCombat EnemyCombat { get; private set; }
    public EnemyReaction EnemyReact { get; private set; }
    public EnemyStats EnemyStats { get; private set; }

    public EnemyGroundedState GroundedState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyHurtState HurtState { get; private set; }
    public EnemyDeadState DeadState { get; private set; }

    void Awake()
    {
        EnemyController = GetComponent<EnemyController>();
        EnemyCombat = GetComponent<EnemyCombat>();
        EnemyReact = GetComponent<EnemyReaction>();
        EnemyStats = GetComponent<EnemyStats>();

        GroundedState = new EnemyGroundedState(this);
        AttackState = new EnemyAttackState(this);
        HurtState = new EnemyHurtState(this);
        DeadState = new EnemyDeadState(this);

        ChangeState(GroundedState);
    }
    void Update() => CurrentState?.Tick();

    public void ChangeState(IEnemyState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
}
