using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float attackTimer = 0f;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        attackTimer = 0f;

        ctx.EnemyCombat?.RequestAttack(ctx.EnemyController?.Player);
    }
    public override void Tick()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= ctx.EnemyStats.AttackCooldown)
            ctx.ChangeState(ctx.GroundedState);
    }
}
