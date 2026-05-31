using TMPro;
using UnityEngine;
using DG.Tweening;

public class ChangeTMPStyle : MonoBehaviour
{
    [SerializeField] private PauseMenuRouter router;

    private RectTransform _tmpRect;
    private TextMeshProUGUI _tmpText;

    private void Awake()
    {
        _tmpRect = GetComponent<RectTransform>();
        _tmpText = GetComponent<TextMeshProUGUI>();
    }

    public void Hover()
    {
        if (_tmpText.color != router.ActiveColor)
            _tmpText.color = router.HoverColor;
        _tmpRect?.DOKill();
        _tmpRect.DOAnchorPosX(20, router.BtnHoverDuration).SetEase(Ease.InQuad);
    }
    public void Unhover()
    {
        if (_tmpText.color != router.ActiveColor)
            _tmpText.color = router.UnhoverColor;
        _tmpRect?.DOKill();
        _tmpRect.DOAnchorPosX(0, router.BtnHoverDuration).SetEase(Ease.InQuad);
    }

    /// <summary>
    /// 此按鈕被點擊 ; 此函數給其他腳本呼叫，並非由 Component_EventTrigger 觸發。
    /// </summary>
    public void BtnActive()
    {
        if (_tmpText != null)
        {
            _tmpText.color = router.ActiveColor;
        }
    }

    /// <summary>
    /// 別的按鈕被點擊 ; ; 此函數給其他腳本呼叫，並非由 Component_EventTrigger 觸發。
    /// </summary>
    public void BtnInactive()
    {
        if (_tmpText != null)
        {
            _tmpText.color = router.UnhoverColor;
        }
    }
}
