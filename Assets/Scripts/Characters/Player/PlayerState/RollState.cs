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
    }
    public override void Tick()
    {
        rollTimer += Time.deltaTime;
        if (!ctx.PlayerMov.isRolling)
            ctx.ChangeState(ctx.GroundedState);
    }
    public override void Exit()
    {
        ctx.PlayerMov.TriggerRootMotion(false);
        // ¨¾¤î°Êµe³Q¤ÁÂ_
        ctx.PlayerMov.OnAnimationEvent_StopRoll();
        ctx.PlayerReact.OnAnimationEvent_SetInvincible(false);
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
