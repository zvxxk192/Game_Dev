using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class LoadingView : BaseUISequenceView
{
    public float InFadeDuration => inFadeDuration;
    public float OutFadeDuration => outFadeDuration;

    [Header("UI Element")]
    [SerializeField] private RectTransform silderHandleRect;
    [SerializeField] private Image titleIcon;


    protected override void OnOpenComplete()
    {
        silderHandleRect.DORotate(new Vector3(0f, 0f, -360f), 3f, RotateMode.FastBeyond360)
                        .SetEase(Ease.Linear)
                        .SetLoops(-1, LoopType.Restart)
                        .SetLink(silderHandleRect.gameObject);

        titleIcon.DOFade(0.3f, 0.9f)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetLink(titleIcon.gameObject);
    }
}
