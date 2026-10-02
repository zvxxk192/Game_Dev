using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UnityEngine.Object.FindFirstObjectByType<GameManager>();
            }
            return _instance;
        }
    }

    public GameContext gameContext { get; private set; }    // 給 GameStateManager 用的上下文


    public void InitializeNewScene(PlayingWorldSceneContext sceneContext)
    {
        if (sceneContext == null)
        {
            Debug.LogWarning("GlobalUIManager: 此場景沒有配置 SceneContext");
            return;
        }

        GameObject newPlayer = sceneContext.LevelPlayer;

        if (sceneContext.LevelPlayer != null)
            gameContext.Setup(newPlayer);
    }
}
