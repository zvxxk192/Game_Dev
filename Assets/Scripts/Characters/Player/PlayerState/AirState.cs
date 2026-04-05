using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
    private bool hasLeftGround = false;
    // 給一個極短的緩衝時間，確保物理引擎更新
    private float minAirTime = 0.1f;
    private float airTimer;

    public PlayerAirState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Exit()
    {
        hasLeftGround = false;
        airTimer = 0f;
    }

    public override void Tick()
    {
        airTimer += Time.deltaTime;

        ctx.PlayerMov.RequestMove();

        if (!hasLeftGround && !ctx.PlayerMov.isGrounded && airTimer > minAirTime)
        {
            hasLeftGround = true;
        }
        if(hasLeftGround && ctx.PlayerMov.isGrounded)
        {
            ctx.ChangeState(ctx.GroundedState);
        }
    }
}
