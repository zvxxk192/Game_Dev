using UnityEngine;

public class EnemyGroundedState : EnemyBaseState
{
    public EnemyGroundedState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Exit()
    {
        ctx.EnemyController?.RequestStopMoving();
    }
    public override void Tick()
    {
        if (ctx.EnemyController.distSqr <= ctx.Data.AttackRadius * ctx.Data.AttackRadius)
        {
            // ª¬ºA¤@ : §ðÀ»
            ctx.ChangeState(ctx.AttackState);
        }
        else if (ctx.EnemyController.distSqr <= ctx.Data.LookRadius * ctx.Data.LookRadius)
        {
            // ª¬ºA¤G : °l³v
            ctx.EnemyController.RequestChasePlayer();
        }
        else
        {
            // ª¬ºA¤T : ¨µÅÞ
            ctx.EnemyController.RequestPatrol();
        }
    }
}
