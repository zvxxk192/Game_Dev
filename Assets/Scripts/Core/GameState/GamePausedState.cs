using UnityEngine;

public class GamePausedState : GameBaseState
{
    public GamePausedState(GameContext gameContext) : base(gameContext) { }

    public override void Enter()
    {
        Time.timeScale = 0f;
    }
    public override void Exit()
    {
        Time.timeScale = 1f;
    }
}
