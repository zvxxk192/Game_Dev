using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    public override void Tick()
    {
        ctx.PlayerMov.RequestMove();

        // °»´ú±¼¸¨
        if (!ctx.PlayerMov.isGrounded)
        {
            ctx.ChangeState(ctx.AirState);
        }
    }

    public override void HandleInput(PlayerCommand command)
    {
        switch (command)
        {
            case PlayerCommand.Attack:
                ctx.ChangeState(ctx.AttackState);
                break;
            case PlayerCommand.Roll:
                ctx.ChangeState(ctx.RollState);
                break;
            case PlayerCommand.Jump:
                ctx.PlayerMov.RequestJump();
                ctx.ChangeState(ctx.AirState);
                break;
        }
    }
}
