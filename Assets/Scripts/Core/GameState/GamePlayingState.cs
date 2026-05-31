using UnityEngine;

public class GamePlayingState : GameBaseState
{
    public GamePlayingState(GameStateManager stateManager) : base(stateManager) { }
    
    public override void Enter()
    {
        Time.timeScale = 1.0f;
    }
}
