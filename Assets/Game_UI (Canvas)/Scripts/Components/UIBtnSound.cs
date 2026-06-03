using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIBtnSound : MonoBehaviour, IPointerEnterHandler
{
    [Header("Audio Settings")]
    [SerializeField] private AudioEvent hoverSoundEvent;
    [SerializeField] private AudioEvent clickSoundEvent;


    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() => PlaySound(clickSoundEvent));
    }
    public void OnPointerEnter(PointerEventData eventData) => PlaySound(hoverSoundEvent);

    void PlaySound(AudioEvent audioEvent)
    {
        AudioManager.Instance.PlayAudioEvent(audioEvent);
    }
}
