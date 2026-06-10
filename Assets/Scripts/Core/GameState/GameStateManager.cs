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
    private GameContext gameContext;

    [SerializeField] private GameStateCommand InitialState = GameStateCommand.Playing;

    [Header("State")]
    public IGameState CurrentState { get; private set; }

    public GamePausedState GamePausedState { get; private set; }
    public GamePlayingState GamePlayingState { get; private set; }
    public GameOverState GameOverState { get; private set; }
    public GameLoadingState GameLoadingState { get; private set; }

    public event Action<IGameState> OnGameStateChanged;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(transform.root.gameObject);

        gameContext = GetComponent<GameContext>();

        if (gameContext != null)
        {
            GamePausedState = new GamePausedState(gameContext);
            GamePlayingState = new GamePlayingState(gameContext);
            GameOverState = new GameOverState(gameContext);
            GameLoadingState = new GameLoadingState(gameContext);
        }

        switch (InitialState)
        {
            case GameStateCommand.Playing:
                ChangeState(GamePlayingState);
                break;
            case GameStateCommand.Paused:
                ChangeState(GamePausedState);
                break;
            case GameStateCommand.GameOver:
                ChangeState(GameOverState);
                break;
            case GameStateCommand.Loading:
                ChangeState(GameLoadingState);
                break;
            default: break;
        }
    }

    private void Update() => CurrentState?.Tick();

    public void ChangeState(IGameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();

        if (newState == GamePlayingState)
            InitialState = GameStateCommand.Playing;
        if (newState == GamePausedState)
            InitialState = GameStateCommand.Paused;
        if (newState == GameOverState)
            InitialState = GameStateCommand.GameOver;
        if (newState == GameLoadingState)
            InitialState = GameStateCommand.Loading;

        OnGameStateChanged?.Invoke(newState);
    }
}
