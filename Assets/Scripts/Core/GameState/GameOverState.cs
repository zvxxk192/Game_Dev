
public class GameOverState : GameBaseState
{
    public GameOverState(GameStateManager stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        UnityEngine.Time.timeScale = 0f;
    }

    public override void Exit()
    {
        UnityEngine.Time.timeScale = 1f;
    }
}
