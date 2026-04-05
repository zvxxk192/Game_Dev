using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private float attackTimer = 0f;
    private float minAttackTime = 0.1f;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        ctx.PlayerMov.TriggerRootMotion(true);

        if (ctx.WeaponController.currentWeapon != null)
            ctx.WeaponController.currentWeapon.RequestAttack();
    }
    public override void Exit()
    {
        attackTimer = 0f;
        ctx.PlayerMov.TriggerRootMotion(false);
    }
    public override void Tick()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer < 0.2f)
            ctx.LockSystem.FaceTargetWhenAttack();

        if (!ctx.WeaponController.currentWeapon.IsAttacking)
            ctx.ChangeState(ctx.GroundedState);
    }
    public override void HandleInput(PlayerCommand command)
    {
        switch (command)
        {
            case PlayerCommand.Attack:
                if (ctx.WeaponController.currentWeapon != null)
                    ctx.WeaponController.currentWeapon.RequestAttack();
                attackTimer = 0f;
                break;
            case PlayerCommand.Roll:
                if(attackTimer > minAttackTime)
                    ctx.ChangeState(ctx.RollState);
                break;
        }
    }
}
