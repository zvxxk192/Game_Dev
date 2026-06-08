using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UnityEngine.Object.FindFirstObjectByType<UIManager>();
            }
            return _instance;
        }
    }

    [Header("已註冊的 UI WindowView 節點")]
    [SerializeField] private BaseUISequenceView pauseWindow;
    [SerializeField] private BaseUISequenceView gameOverWindow;

    void Awake()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);
    }
    void Start()
    {
        // 讓 UIManager 去聽全域遊戲狀態
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged += HandleGameState;
        }
    }
    void OnDisable()
    {
        // 讓 UIManager 去聽全域遊戲狀態
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged -= HandleGameState;
        }
    }

    void HandleGameState(IGameState state)
    {
        if (state == GameStateManager.Instance.GamePausedState)
            pauseWindow.OpenPanel();
        else if (state == GameStateManager.Instance.GamePlayingState)
            pauseWindow.ClosePanel();
        else if (state == GameStateManager.Instance.GameOverState)
            gameOverWindow.OpenPanel();
    }
}
