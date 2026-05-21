using UnityEngine;
using UnityEngine.UIElements;

public class UIAudioManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    private AudioSource audioSource;

    private UIDocument uiDocument;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        uiDocument = GetComponent<UIDocument>();
    }
    void Start()
    {
        if (uiDocument == null) return;

        var root = uiDocument.rootVisualElement;
        var buttons = root.Query<Button>().ToList();

        foreach (var btn in buttons)
        {
            btn.RegisterCallback<MouseEnterEvent>(evt => PlaySound(hoverSound));
            btn.RegisterCallback<ClickEvent>(evt => PlaySound(clickSound));
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
