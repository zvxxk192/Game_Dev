using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        ctx.EnemyReact?.InstantiateDeadLoot();
        ctx.EnemyController?.RequestDie();
    }
}
