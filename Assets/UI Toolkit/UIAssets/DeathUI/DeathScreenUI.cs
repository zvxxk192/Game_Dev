using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DeathScreenUI : BaseView
{
    private VisualElement deathScreen;

    protected override void OnBindElements()
    {
        deathScreen = Root.Q("death-screen");
    }
    protected override void OnRegisterEvents()
    {
        GameStateManager.Instance.OnGameStateChanged += HandlePlayerDeath;
    }
    protected override void OnUnregisterEvents()
    {
        GameStateManager.Instance.OnGameStateChanged -= HandlePlayerDeath;
    }

    void HandlePlayerDeath(GameState newState)
    {
        if (newState == GameState.PlayerDead)
        {
            deathScreen.AddToClassList("death-screen--active");
        }
        else
        {
            deathScreen.RemoveFromClassList("death-screen--active");
        }
    }
    void RestartCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
