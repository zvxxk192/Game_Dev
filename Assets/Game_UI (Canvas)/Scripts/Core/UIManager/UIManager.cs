using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("需要外部物件的 UI Element")]
    [SerializeField] private StatusTextController statusTextController;
    [SerializeField] private HealthBarUI healthBarUI;


    private BaseUISequenceView currentView; 

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        // 讓 UIManager 去聽全域遊戲狀態
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged += HandleGameState;
        }
    }
    private void OnDisable()
    {
        // 讓 UIManager 去聽全域遊戲狀態
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged -= HandleGameState;
        }
    }

    public void PrepareForSceneChange()
    {
        healthBarUI.gameObject.SetActive(false);
        statusTextController.gameObject.SetActive(false);
    }
    public void InitializeNewScene(PlayingWorldSceneContext sceneContext)
    {
        if (sceneContext == null)
        {
            Debug.LogWarning("UIManager: 此場景沒有配置 SceneContext");
            return;
        }

        GameObject newPlayer = sceneContext.LevelPlayer;

        if (newPlayer != null)
        {
            healthBarUI.Setup(newPlayer);
            statusTextController.Setup(newPlayer);
        }
    }

    private void HandleGameState(IGameState state)
    {
        if (state == GameStateManager.Instance.GamePausedState)
        {
            pauseWindow.OpenPanel();
            currentView = pauseWindow;
        }
        else if (state == GameStateManager.Instance.GamePlayingState)
        {
            if (currentView != null)
                currentView.ClosePanel();
            currentView = null;
        }
        else if (state == GameStateManager.Instance.GameOverState)
        {
            if (currentView != null)
                currentView.ClosePanel();
            currentView = gameOverWindow;

            gameOverWindow.OpenPanel();
        }
    }
}
