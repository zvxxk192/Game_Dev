using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public abstract class BaseUISequenceView : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    private Sequence currentSequence;

    [Header("UI 基礎設定")]
    [SerializeField] protected float bufferDuration = 0.2f;  // 防止動畫執行期間亂點
    [SerializeField] protected float fadeDuration = 0.4f;
    [SerializeField] protected Ease openEase = Ease.OutCubic;
    [SerializeField] protected Ease closeEase = Ease.InCubic;

    public bool IsOpen { get; private set; }

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 開啟面板（對外接口）
    /// </summary>
    public void OpenPanel()
    {
        if (IsOpen) return;
        IsOpen = true;

        // 先殺掉正在跑的動畫，防止衝突
        currentSequence?.Kill(true);

        // 顯示基本設定（允許點擊）
        gameObject.SetActive(true);

        currentSequence = DOTween.Sequence().SetLink(gameObject).SetUpdate(true);

        // 淡入
        currentSequence.Append(canvasGroup.DOFade(1f, fadeDuration).SetEase(openEase));

        // 讓子類別去塞入自己專屬的動畫
        OnBuildOpenSequence(currentSequence);

        currentSequence.OnComplete(() =>
        {
            // 延遲互動，防止玩家在淡入時亂點
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            OnOpenComplete();
        });
    }

    /// <summary>
    /// 關閉面板（對外接口）
    /// </summary>
    public void ClosePanel()
    {
        if (!IsOpen) return;
        IsOpen = false;

        // 關閉互動，防止玩家在淡出時亂點
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        currentSequence?.Kill(true);
        currentSequence = DOTween.Sequence().SetLink(gameObject).SetUpdate(true);

        // 淡出
        currentSequence.Append(canvasGroup.DOFade(0f, fadeDuration).SetEase(closeEase));

        // 子類別專屬的關閉動畫
        OnBuildCloseSequence(currentSequence);

        // 動畫播完後關閉 GameObject
        currentSequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
            OnCloseComplete();
        });
    }


    protected virtual void OnBuildOpenSequence(Sequence seq) { }
    protected virtual void OnBuildCloseSequence(Sequence seq) { }
    protected virtual void OnOpenComplete() { }
    protected virtual void OnCloseComplete() { }
}
