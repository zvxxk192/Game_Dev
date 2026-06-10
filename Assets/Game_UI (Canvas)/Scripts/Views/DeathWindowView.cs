using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DeathWindowView : BaseUISequenceView
{
    [Header("Scale Component")]
    [SerializeField] private TextMeshProUGUI gameOverTitle;
    [SerializeField] private float titleFadeDuration = 1f;

    [Header("Button Component")]
    [SerializeField] private CanvasGroup respawnBtnCanvas;
    [SerializeField] private Button respawnBtn;
    [SerializeField] private TextMeshProUGUI respawnText;
    [SerializeField] private Image respawnIcon;
    [SerializeField] private float respawnBtnFadeDuration = 0.5f;
    [SerializeField] private float respawnTextLoopDuration = 0.5f;


    protected override void Awake()
    {
        base.Awake();

        gameOverTitle.fontSize = 0;
        respawnBtnCanvas.alpha = 0;
    }
    protected override void OnBuildOpenSequence(Sequence seq)
    {
        seq.Append(gameOverTitle.DOFontSize(160, titleFadeDuration).SetEase(openEase));

        seq.Append(respawnBtnCanvas.DOFade(1f, respawnBtnFadeDuration));

        seq.AppendInterval(0.5f);
    }
    protected override void OnOpenComplete()
    {
        respawnIcon.DOFade(0.2f, respawnTextLoopDuration).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        respawnText.DOFade(0.2f, respawnTextLoopDuration).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }
}
