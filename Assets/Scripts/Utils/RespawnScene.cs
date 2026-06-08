using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnScene : MonoBehaviour
{
    public void OnFullRespawnSceneBtnClick()
    {
        Time.timeScale = 1f;

        // 抓去當前活躍場景的 index ，並廣播即將重新載入
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        GameEvents.OnRequestSceneLoad(currentSceneIndex);
    }
}
