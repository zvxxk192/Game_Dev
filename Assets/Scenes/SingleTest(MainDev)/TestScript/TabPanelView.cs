using UnityEngine;
using DG.Tweening;

public class TabPanelView : BaseUISequenceView
{
    [Header("滑動動畫設定")]
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private float slideOffset = 50f; // 從下方多遠滑上來

    protected override void Awake()
    {
        base.Awake();
        if (panelRect == null) panelRect = GetComponent<RectTransform>();
    }

    protected override void OnBuildOpenSequence(Sequence seq)
    {
        // 確保進場前，面板在稍微偏下方的位置
        panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, -slideOffset);

        // 使用 Join，讓往上滑的動畫跟底層的 DOFade(淡入) 同時執行
        seq.Join(panelRect.DOAnchorPosY(0f, fadeDuration).SetEase(openEase));
    }

    protected override void OnBuildCloseSequence(Sequence seq)
    {
        // 退場時，面板稍微往下滑
        seq.Join(panelRect.DOAnchorPosY(-slideOffset, fadeDuration).SetEase(closeEase));
    }
}
