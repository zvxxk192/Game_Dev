using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("API References")]
    [SerializeField] private RespawnScene respawnSceneApi;

    [Header("Btn Components")]
    [SerializeField] private Button startBtn;
    [SerializeField] private Button loadBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button endBtn;

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
    }

    private void OnClickStartBtn()
    {
        respawnSceneApi.OnFullRespawnSceneBtnClick(targetSceneName);
    }
    private void OnClickLoadBtn()
    {

    }
    private void OnClickSettingsBtn()
    {

    }
    private void OnClickQuitBtn()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
