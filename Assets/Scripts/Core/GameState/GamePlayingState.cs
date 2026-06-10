using UnityEngine;

public class GamePlayingState : GameBaseState
{
    public GamePlayingState(GameContext gameContext) : base(gameContext) { }

    public override void Enter()
    {
        Time.timeScale = 1.0f;
    }
}
