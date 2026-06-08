using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("UI Element")]
    [SerializeField] private LoadingView loadingView;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressPercent;

    [Header("Loading Settings")]
    [SerializeField] private float minLoadingTime;

    private void Awake()
    {
        // 為跨場景加載物件
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        GameEvents.OnRequestSceneLoad += LoadScene;
    }
    private void OnDisable()
    {
        GameEvents.OnRequestSceneLoad -= LoadScene;
    }

    private void LoadScene(int sceneIndex)
    {
        // 在場景加載前的前置作業
        UIManager.Instance.PrepareForSceneChange();
        StartCoroutine(LoadSceneRoutine(sceneIndex));
    }
    private IEnumerator LoadSceneRoutine(int sceneIndex)
    {
        // 確保不受死亡畫面等暫停時間的 View 影響，恢復時間並開始淡入
        Time.timeScale = 1.0f;
        loadingView.OpenPanel();

        // 等待淡入動畫播完
        yield return new WaitForSecondsRealtime(loadingView.FadeDuration);

        // 開始異步加載場景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // 阻止場景讀取完畢後馬上切換
        asyncLoad.allowSceneActivation = false;

        // 更新進度
        while (!asyncLoad.isDone)
        {
            // asyncLoad.progress 最多只能跑到 0.9
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;
            if (progressPercent != null)
                progressPercent.text = ((int)(progress * 100)) + "%";

            if (asyncLoad.progress >= 0.9f)
                asyncLoad.allowSceneActivation = true;

            yield return null;
        }

        // 在場景加載完後開始加載物件
        PlayingWorldSceneContext currentContext = Object.FindFirstObjectByType<PlayingWorldSceneContext>();
        // 傳給中央頭腦
        if (UIManager.Instance != null)
            UIManager.Instance.InitializeNewScene(currentContext);

        // 淡出
        loadingView.ClosePanel();
    }
}
