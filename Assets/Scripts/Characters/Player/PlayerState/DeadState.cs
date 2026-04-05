using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        ctx.PlayerMov.TriggerRootMotion(true);
        ctx.PlayerReact.RequestDie();
    }
}
