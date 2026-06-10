using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalUIManager : MonoBehaviour
{
    private static GlobalUIManager _instance;
    public static GlobalUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UnityEngine.Object.FindFirstObjectByType<GlobalUIManager>();
            }
            return _instance;
        }
    }

    [Header("需要局部依賴的全域 UI 組件")]
    //[SerializeField] private StatusTextController statusTextController;
    //[SerializeField] private HealthBarUI healthBarUI;
    [SerializeField] private GameContext gameContext;    // 給 GameStateManager 用的上下文

    [Header("刷新場景的唯一 API")]
    [field: SerializeField] public RespawnScene RespawnSceneApi { get; private set; }

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PrepareForSceneChange()
    {
        //if (healthBarUI  != null)
        //    healthBarUI.gameObject.SetActive(false);
        //if (statusTextController != null)
        //    statusTextController.gameObject.SetActive(false);
    }
    public void InitializeNewScene(PlayingWorldSceneContext sceneContext)        
    {
        if (sceneContext == null)
        {
            Debug.LogWarning("GlobalUIManager: 此場景沒有配置 SceneContext");
            return;
        }

        GameObject newPlayer = sceneContext.LevelPlayer;

        if (newPlayer != null)
        {
            //healthBarUI.Setup(newPlayer);
            //statusTextController.Setup(newPlayer);
            gameContext.Setup(newPlayer);
        }
    }
}
