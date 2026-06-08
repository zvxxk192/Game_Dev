using TMPro;
using DG.Tweening;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LocalTextsScrollFader : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI textA;
    [SerializeField] private TextMeshProUGUI textB;

    [Header("Tips Settings")]
    [SerializeField, TextArea(2, 4)] private string[] tips;
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private float fadeDuration = 0.8f;

    [Header("Offset Settings")]
    [SerializeField] private Vector2 transitionOffset;

    private Coroutine loopCoroutine;
    private int currentIndex = 0;
    private bool isTextA_Active = true; // 用來記錄現在是誰在畫面上

    private bool hasRecoredPos = false;
    private Vector2 basePosition = Vector2.zero;

    private void Awake()
    {
        RecordBasePosition();
    }
    private void RecordBasePosition()
    {
        if (hasRecoredPos) return;

        LayoutRebuilder.ForceRebuildLayoutImmediate(textA.transform.parent as RectTransform);
        basePosition = textA.rectTransform.anchoredPosition;
        hasRecoredPos = true;
    }
    private void OnEnable()
    {
        if (tips.Length == 0) return;

        // Awake 可能沒讀到
        RecordBasePosition();

        // 先把兩個文字都變透明
        ResetText(textA);
        ResetText(textB);

        // 先讓 TextA 顯示第一句話
        textA.text = tips[currentIndex];
        textA.color = new Color(1, 1, 1, 1);
        textA.rectTransform.anchoredPosition = basePosition;

        loopCoroutine = StartCoroutine(TipLoopRoutine());
    }
    private void OnDisable()
    {
        if (loopCoroutine != null) StopCoroutine(loopCoroutine);
        textA.DOKill();
        textB.DOKill();
    }

    private void ResetText(TextMeshProUGUI txt)
    {
        Color c = txt.color;
        c.a = 0f;
        txt.color = c;
    }

    private IEnumerator TipLoopRoutine()
    {
        // 第一次先等個幾秒再開始切換
        yield return new WaitForSecondsRealtime(displayDuration);

        while (true)
        {
            // 決定誰是「現在準備退場的舊文字」，誰是「準備進場的新文字」
            TextMeshProUGUI currentText = isTextA_Active ? textA : textB;
            TextMeshProUGUI nextText = isTextA_Active ? textB : textA;

            // 準備新文字的內容
            currentIndex = (currentIndex + 1) % tips.Length;
            nextText.text = tips[currentIndex];

            // 先把新文字瞬間移動到「正下方」，並保持透明
            nextText.rectTransform.anchoredPosition = basePosition - transitionOffset;
            ResetText(nextText);

            // 建立一個 Sequence 讓進場和退場「同時」發生
            Sequence seq = DOTween.Sequence().SetUpdate(true).SetLink(gameObject);

            // 舊文字：往上移動 + 淡出
            seq.Join(currentText.rectTransform.DOAnchorPos(basePosition + transitionOffset, fadeDuration).SetEase(Ease.InOutSine));
            seq.Join(currentText.DOFade(0f, fadeDuration));

            // 新文字：從下方回到正中間並淡入
            seq.Join(nextText.rectTransform.DOAnchorPos(basePosition, fadeDuration).SetEase(Ease.OutBack));
            seq.Join(nextText.DOFade(1f, fadeDuration));

            // 等待這段超帥的動畫播完
            yield return seq.WaitForCompletion();

            // 交換身份
            isTextA_Active = !isTextA_Active;

            // 停留在畫面上給玩家閱讀
            yield return new WaitForSecondsRealtime(displayDuration);
        }
    }
}