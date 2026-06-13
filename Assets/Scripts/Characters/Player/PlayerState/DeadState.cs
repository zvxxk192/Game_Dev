using UnityEngine;
using static Coffee.UIExtensions.UIParticleAttractor;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private float deathAnimationDuration = 3.0f;
    private float deadTimer;

    public override void Enter()
    {
        deadTimer = 0f;

        // 確保執行動畫採用動畫的位移
        ctx.PlayerMov.TriggerRootMotion(true);
        ctx.PlayerMov.SetDisplacementEnabled(true);
        ctx.PlayerReact.RequestDie();
    }
    public override void Tick()
    {
        deadTimer += Time.deltaTime;

        if (deadTimer > deathAnimationDuration)
        {
            GameStateManager.Instance.ChangeState(GameStateManager.Instance.GameOverState);
        }
    }
}
