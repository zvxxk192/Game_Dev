using UnityEngine;

public class PlayerHurtState : PlayerBaseState
{
    private float hurtDuration = 0.6f;
    private float minStunTime = 0.1f; //最短硬直時間，防止bug
    private float hurtTimer = 0f;

    public PlayerHurtState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        hurtTimer = 0f;
        ctx.PlayerReact?.RequestHurt(ctx.PlayerReact.LastAttackerPos);
    }
    //public override void Exit()
    //{
    //    //防治打斷後未觸發到的AnimationEvent
    //    ctx.PlayerReact.OnAnimationEvent_SetInvincible(false);
    //    hurtTimer = 0f;
    //}
    public override void Tick()
    {
        hurtTimer += Time.deltaTime;
        if(hurtTimer >= hurtDuration)
        {
            ctx.ChangeState(ctx.GroundedState);
        }
    }
    public override void HandleInput(PlayerCommand command)
    {
        if (command == PlayerCommand.Roll && hurtTimer > minStunTime)
            ctx.ChangeState(ctx.RollState);
    }
}
