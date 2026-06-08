using System.Threading;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private float deathAnimationDuration = 3.0f;
    private float deadTimer;

    public override void Enter()
    {
        deadTimer = 0f;

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
