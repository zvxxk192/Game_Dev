using UnityEngine;
using DG.Tweening;

public class TabPanelView : BaseUISequenceView
{
    [Header("滑動動畫設定")]
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private float slideOffset = 60f;
    [SerializeField] private float moveDuration = 0.2f;
    protected override void Awake()
    {
        base.Awake();
        if (panelRect == null) panelRect = GetComponent<RectTransform>();
    }

    protected override void OnBuildOpenSequence(Sequence seq)
    {
        // 確保進場前，面板在偏 offset 的位置
        panelRect.anchoredPosition = new Vector2(slideOffset, panelRect.anchoredPosition.y);

        // 使用 Join，讓動畫跟底層的 DOFade(淡入) 同時執行
        seq.Join(panelRect.DOAnchorPosX(0f, moveDuration).SetEase(openEase));
    }

    protected override void OnBuildCloseSequence(Sequence seq)
    {
        // 退場時，面板稍微往 -offset 滑
        seq.Join(panelRect.DOAnchorPosX(-slideOffset, moveDuration).SetEase(closeEase));
    }
}
