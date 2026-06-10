
public class GameOverState : GameBaseState
{
    public GameOverState(GameContext gameContext) : base(gameContext) { }

    public override void Enter()
    {
        UnityEngine.Time.timeScale = 0f;
    }

    public override void Exit()
    {
        UnityEngine.Time.timeScale = 1f;
    }
}
