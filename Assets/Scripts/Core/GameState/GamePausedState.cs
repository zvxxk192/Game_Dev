using UnityEngine;

public class GamePausedState : GameBaseState
{
    public GamePausedState(GameStateManager stateManager) : base(stateManager) { }

    public override void Enter()
    {
        Time.timeScale = 0f;
    }
}
