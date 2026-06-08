using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RespawnBtn : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float respawnTextLoopDuration = 0.5f;
    [SerializeField] private TextMeshProUGUI respawnText;
    [SerializeField] private Image respawnIcon;
    [SerializeField] private RespawnScene respawnScene;

    public void Awake()
    {
        respawnScene = GetComponent<RespawnScene>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        respawnText.DOKill();
        respawnText.alpha = 1;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        respawnIcon.DOFade(0.2f, respawnTextLoopDuration).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        respawnText.DOFade(0.2f, respawnTextLoopDuration).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }
    public void OnPointerUp(PointerEventData eventData)
        => respawnScene.OnFullRespawnSceneBtnClick();
}
