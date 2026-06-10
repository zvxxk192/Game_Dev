using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayingWorldUIManager : MonoBehaviour
{
    [Header("已註冊的 UI WindowView 節點")]
    [SerializeField] private BaseUISequenceView pauseWindow_View;
    [SerializeField] private BaseUISequenceView gameOver_View;


    private BaseUISequenceView currentView;
    
    private void OnEnable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged += HandleGameStateView;
        }
    }
    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateView;
        }
    }

    private void HandleGameStateView(IGameState state)
    {
        if (state == GameStateManager.Instance.GamePausedState)
        {
            pauseWindow_View.OpenPanel();
            currentView = pauseWindow_View;
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

            gameOver_View.OpenPanel();
            currentView = gameOver_View;
        }
        else if (state == GameStateManager.Instance.GameLoadingState)
        {
            // LoadingView 為全域場景，這裡的局部 UIManager 只負責關閉被覆蓋頁面
            if (currentView != null)
                currentView.ClosePanel();
        }
    }
}
