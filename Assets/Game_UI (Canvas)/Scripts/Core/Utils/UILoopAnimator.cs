using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class UILoopAnimator : MonoBehaviour
{
    public enum AnimType { Rotate, Scale, MoveLocal, MoveAnchor }

    [System.Serializable]
    public class AnimConfig
    {
        public AnimType type;
        public Vector3 targetValue;
        public float duration = 1f;
        public Ease ease = Ease.InOutSine;
        public LoopType loopType = LoopType.Yoyo;
    }

    [Header("Global Id")]
    public string globalGroupId = "MenuScreenLoops";

    [Header("Configed UI List")]
    public List<AnimConfig> animations = new List<AnimConfig>();

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        // 每次物件啟用時，建立動畫
        StartAllLoops();
    }

    void OnDisable()
    {
        // 物件關閉時，精準殺掉這台物件身上的所有 DOTween
        DOTween.Kill(this);
    }

    private void StartAllLoops()
    {
        // 先確保身上乾淨
        DOTween.Kill(this);

        foreach (var config in animations)
        {
            Tween t = null;

            // 根據配置動態生成對應的 DOTween
            switch (config.type)
            {
                case AnimType.Rotate:
                    t = rectTransform.DORotate(config.targetValue, config.duration);
                    break;
                case AnimType.Scale:
                    t = rectTransform.DOScale(config.targetValue, config.duration);
                    break;
                case AnimType.MoveLocal:
                    t = rectTransform.DOLocalMove(config.targetValue, config.duration);
                    break;
                case AnimType.MoveAnchor:
                    t = rectTransform.DOAnchorPos(config.targetValue, config.duration);
                    break;
            }

            if (t != null)
            {
                // 設定共用參數
                t.SetEase(config.ease)
                 .SetLoops(-1, config.loopType)
                 .SetLink(gameObject) // 自動與 GameObject 生命週期綁定
                 .SetTarget(this)     // 綁定目標為本組件
                 .SetId(globalGroupId); // 打上全局標籤
            }
        }
    }
}
