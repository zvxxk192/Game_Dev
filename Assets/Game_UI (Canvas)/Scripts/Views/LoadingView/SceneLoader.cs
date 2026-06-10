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

    private void LoadScene(string sceneName)
    {
        // 在場景加載前的前置作業
        if (GlobalUIManager.Instance != null)
            GlobalUIManager.Instance.PrepareForSceneChange();
        StartCoroutine(LoadSceneRoutine(sceneName));
    }
    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // 宣告開始載入，以通知 UIManager 關閉現在的頁面
        if (GameStateManager.Instance  != null)
            GameStateManager.Instance.ChangeState(GameStateManager.Instance.GameLoadingState);

        // 開啟載入頁面
        loadingView.OpenPanel();

        // 等待淡入動畫播完
        yield return new WaitForSecondsRealtime(loadingView.InFadeDuration);

        // 開始異步加載場景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

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

        // 在場景加載完後開始加載全域「局部依賴」的物件
        PlayingWorldSceneContext currentContext = Object.FindFirstObjectByType<PlayingWorldSceneContext>();

        // 傳給中央頭腦
        if (GlobalUIManager.Instance != null)
            GlobalUIManager.Instance.InitializeNewScene(currentContext);

        // 宣告加載狀態完畢，以通知 UIManager 初始化
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.ChangeState(GameStateManager.Instance.GamePlayingState);

        // 關閉載入頁面
        loadingView.ClosePanel();

        //加個緩衝防止 Bug
        yield return new WaitForSecondsRealtime(1);
    }
}
