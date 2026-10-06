using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Btn Components")]
    [SerializeField] private Button startBtn;
    [SerializeField] private Button loadBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button endBtn;
    [SerializeField] private Button loadBackBtn;
    [SerializeField] private Button settingsBackBtn;

    [Header("ContentView Components")]
    [SerializeField] private BaseUISequenceView mainPageView;
    [SerializeField] private BaseUISequenceView loadContentView;
    [SerializeField] private BaseUISequenceView settingsContentView;

    [Header("Target Scene Name")]
    [SerializeField] private string targetSceneName;


    private void Start()
    {
        if (startBtn != null)
            startBtn.onClick.AddListener(OnClickStartBtn);
        if (loadBtn != null)
            loadBtn.onClick.AddListener(OnClickLoadBtn);
        if (settingsBtn != null)
            settingsBtn.onClick.AddListener(OnClickSettingsBtn);
        if (endBtn != null)
            endBtn.onClick.AddListener(OnClickQuitBtn);
        if (loadBackBtn != null)
            loadBackBtn.onClick.AddListener(OnClickLoadBackBtn);
        if (settingsBackBtn != null)
            settingsBackBtn.onClick.AddListener(OnClickSettingsBackBtn);

        mainPageView.OpenPanel();
    }

    private void OnDestroy()
    {
        if (startBtn != null)
            startBtn.onClick.RemoveListener(OnClickStartBtn);
        if (loadBtn != null)
            loadBtn.onClick.RemoveListener(OnClickLoadBtn);
        if (settingsBtn != null)
            settingsBtn.onClick.RemoveListener(OnClickSettingsBtn);
        if (endBtn != null)
            endBtn.onClick.RemoveListener(OnClickQuitBtn);
        if (loadBackBtn != null)
            loadBackBtn.onClick.RemoveListener(OnClickLoadBackBtn);
        if (settingsBackBtn != null)
            settingsBackBtn.onClick.RemoveListener(OnClickSettingsBackBtn);
    }

    private void OnClickStartBtn()
    {
        GameEvents.OnRequestSceneLoad(targetSceneName);
    }
    private void OnClickLoadBtn()
    {
        mainPageView.ClosePanel();
        loadContentView.OpenPanel();
    }
    private void OnClickSettingsBtn()
    {
        mainPageView.ClosePanel();
        settingsContentView.OpenPanel();
    }
    private void OnClickQuitBtn()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    private void OnClickLoadBackBtn()
    {
        loadContentView.ClosePanel();
        mainPageView.OpenPanel();
    }
    private void OnClickSettingsBackBtn()
    {
        settingsContentView.ClosePanel();
        mainPageView.OpenPanel();
    }
}
