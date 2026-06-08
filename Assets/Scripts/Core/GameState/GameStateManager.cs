using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;
    public static GameStateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UnityEngine.Object.FindFirstObjectByType<GameStateManager>();
            }
            return _instance;
        }
    }

    [Header("State")]
    public IGameState CurrentState { get; private set; }

    public GamePausedState GamePausedState { get; private set; }
    public GamePlayingState GamePlayingState { get; private set; }
    public GameOverState GameOverState { get; private set; }

    public event Action<IGameState> OnGameStateChanged;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        GamePausedState = new GamePausedState(this);
        GamePlayingState = new GamePlayingState(this);
        GameOverState = new GameOverState(this);

        ChangeState(GamePlayingState);
    }

    private void Update() => CurrentState?.Tick();

    public void ChangeState(IGameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();

        OnGameStateChanged?.Invoke(newState);
    }
}
