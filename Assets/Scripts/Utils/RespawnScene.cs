using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnScene : MonoBehaviour
{
    public void OnFullRespawnSceneBtnClick(string targetSceneName = "")
    {
        Time.timeScale = 1f;

        string finalSceneToLoadName = string.IsNullOrEmpty(targetSceneName) 
            ? SceneManager.GetActiveScene().name : targetSceneName;
        // 抓去當前活躍場景的 index ，並廣播即將重新載入
        GameEvents.OnRequestSceneLoad(finalSceneToLoadName);
    }
}
