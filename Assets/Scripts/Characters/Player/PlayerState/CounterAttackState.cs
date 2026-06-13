using UnityEngine;

public class PlayerCounterAttackState : PlayerBaseState
{
    private float counterAttackTimer = 0f;
    private float minCounterAttackTime = 1f;
    private bool hasIsAttack = false;

    public PlayerCounterAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        counterAttackTimer = 0f;
        ctx.PlayerMov.TriggerRootMotion(true);
        ctx.PlayerMov.SetDisplacementEnabled(true);

        if (ctx.WeaponController.currentWeapon != null) 
            ctx.WeaponController.currentWeapon.RequestCounterAttack();
    }
    public override void Exit()
    {
        ctx.PlayerMov.TriggerRootMotion(false);
        ctx.PlayerMov.SetDisplacementEnabled(false);

        // ¨¾¤î°Êµe³Q¤ÁÂ_
        ctx.PlayerReact.OnAnimationEvent_SetInvincible(false);
    }
    public override void Tick()
    {
        counterAttackTimer += Time.deltaTime;

        if (counterAttackTimer < 0.1f)
            ctx.LockSystem.FaceTargetWhenAttack();

        if (ctx.WeaponController.currentWeapon.IsAttacking)
            hasIsAttack = true;
        else if (!ctx.WeaponController.currentWeapon.IsAttacking && hasIsAttack)
            ctx.ChangeState(ctx.GroundedState);
    }
    public override void HandleInput(PlayerCommand command)
    {
        switch (command)
        {
            case PlayerCommand.Roll:
                if (counterAttackTimer > minCounterAttackTime)
                    ctx.ChangeState(ctx.RollState);
                break;
        }
    }
}
