using UnityEngine;
using DG.Tweening;

public class PauseWindowView : BaseUISequenceView
{
    [Header("視窗縮放設定")]
    [SerializeField] private RectTransform windowRect;
    [SerializeField] private float startScale = 0.9f;

    protected override void Awake()
    {
        base.Awake();
        if (windowRect == null) windowRect = GetComponent<RectTransform>();
    }

    protected override void OnBuildOpenSequence(Sequence seq)
    {
        // 先把大小縮到 95%
        windowRect.localScale = Vector3.one * startScale;

        // 跟著底層的 DOFade 一起放大到 100%
        seq.Join(windowRect.DOScale(Vector3.one, inFadeDuration).SetEase(openEase));
    }

    protected override void OnBuildCloseSequence(Sequence seq)
    {
        // 退場時縮回 95%
        seq.Join(windowRect.DOScale(Vector3.one * startScale, outFadeDuration).SetEase(closeEase));
    }
}
