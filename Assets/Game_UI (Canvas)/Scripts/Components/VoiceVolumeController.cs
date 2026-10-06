using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class VoiceVolumeController : MonoBehaviour
{
    [Header("UI Component")]
    [SerializeField] private Scrollbar volumeScrollbar;
    [SerializeField] private TextMeshProUGUI volumeText;

    //[Header("Voice Component")]
    //[SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        // 根據自身名字劃分職責
        if (gameObject.name == "MasterVoiceController")
        {
            int volumePercentage = Mathf.RoundToInt(AudioManager.Instance.GlobalSfxVolume * 100);
            volumeText.text = volumePercentage.ToString() + "%";
            volumeScrollbar.value = volumePercentage;
        }
        else if (gameObject.name == "MusicVoiceController")
        {
            int volumePercentage = Mathf.RoundToInt(AudioManager.Instance.GlobalBgmVolume * 100);
            volumeText.text = volumePercentage.ToString() + "%";
            volumeScrollbar.value = volumePercentage;
        }
    }
    private void Start()
    {
        if (volumeScrollbar != null)
        {
            volumeScrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

            OnScrollbarValueChanged(volumeScrollbar.value);
        }
    }
    private void OnScrollbarValueChanged(float value)
    {
        int volumePercentage = Mathf.RoundToInt(value * 100);
        if (volumeText != null)
        {
            volumeText.text = volumePercentage.ToString() + "%"; 
        }

        // 根據自身名字劃分職責
        if (gameObject.name == "MasterVoiceController")
        {
            AudioManager.Instance.SetBgmVolume(value);
            AudioManager.Instance.SetSfxVolume(value);
        }
        else if (gameObject.name == "MusicVoiceController")
        {
            AudioManager.Instance.SetBgmVolume(value);
        }
    }

    private void OnDestroy()
    {
        if (volumeScrollbar != null)
        {
            volumeScrollbar.onValueChanged.RemoveListener(OnScrollbarValueChanged);
        }
    }
}
