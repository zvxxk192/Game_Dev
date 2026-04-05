using UnityEngine;

public class EnemyHurtState : EnemyBaseState
{
    private float hurtTimer = 0f;

    public EnemyHurtState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        hurtTimer = 0f;
        ctx.EnemyController?.RequestStagger(ctx.EnemyReact.LastAttackerPos);
    }
    public override void Tick()
    {
        hurtTimer += Time.deltaTime;

        if (hurtTimer >= ctx.EnemyStats.AttackCooldown)
            ctx.ChangeState(ctx.GroundedState);
    }
}
