using UnityEngine;

public class PlayerRollState : PlayerBaseState
{
    private float minRollTime = 0.7f;
    private float rollTimer = 0f;

    public PlayerRollState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        rollTimer = 0f;
        ctx.PlayerMov.TriggerRootMotion(true);
        ctx.PlayerMov.RequestRoll();
        // 防止走斜坡會飄起來
        ctx.PlayerMov.ResizeCollider(1f);
    }
    public override void Tick()
    {
        rollTimer += Time.deltaTime;
        if (!ctx.PlayerMov.isRolling)
        {
            ctx.ChangeState(ctx.GroundedState);

            // 只有當沒有要反擊的時候才需要關掉無敵幀
            ctx.PlayerReact.OnAnimationEvent_SetInvincible(false);
        }
    }
    public override void Exit()
    {
        ctx.PlayerMov.TriggerRootMotion(false);
        ctx.PlayerMov.OnAnimationEvent_StopRoll();  // 防止動畫被切斷
        ctx.PlayerMov.ResizeCollider(2f); // 恢復
    }
    public override void HandleInput(PlayerCommand command)
    {
        switch (command)
        {
            case PlayerCommand.CounterAttack:
                if (rollTimer > minRollTime)
                    ctx.ChangeState(ctx.CounterAttackState);
                break;
        }
    }
}
