using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image DamageBufferImage;
    [SerializeField] private Image RecoverBufferImage;
    [SerializeField] private Image HealthFillImage;

    [SerializeField] private PlayerEventsManager events;

    private float _currentHp;
    private float _maxHp;

    private void OnEnable()
    {
        events.OnPlayerHpChanged += HandleHealthBarChanged;
    }
    private void OnDisable()
    {
        events.OnPlayerHpChanged -= HandleHealthBarChanged;
    }

    private void HandleHealthBarChanged(float currentHp, float maxHp, bool isIncrease)
    {
        _currentHp = currentHp;
        _maxHp = maxHp;

        if (isIncrease)
            HealthIncrease(HealthFillImage, DamageBufferImage, RecoverBufferImage);
        else
            HealthDecrease(HealthFillImage, RecoverBufferImage, DamageBufferImage);
    }
    private void HealthDecrease(Image fill, Image follow, Image buffer)
    {
        // 確保被頻繁切換不突兀
        fill?.DOKill(true);
        follow?.DOKill(true);
        buffer?.DOKill(true);

        float targetHpPercent = _currentHp / _maxHp;

        // 確保不被上次動畫影響
        buffer.DOFade(1f, 0f);

        // 紅條、綠條啟動
        fill.DOFillAmount(targetHpPercent, 0.5f);
        follow.DOFillAmount(targetHpPercent, 0.5f);

        // 白條緊跟其後並淡出
        buffer.DOFillAmount(targetHpPercent, 2f);
        buffer.DOFade(0, 2f);
    }
    private void HealthIncrease(Image fill, Image follow, Image buffer)
    {
        // 確保被頻繁切換不突兀
        fill?.DOKill(true);
        follow?.DOKill(true);
        buffer?.DOKill(true);

        float targetHpPercent = _currentHp / _maxHp;

        // 確保不被上次動畫影響
        buffer.DOFade(1f, 0f);

        // 綠條啟動並淡出
        buffer.DOFillAmount(targetHpPercent, 0.5f);
        buffer.DOFade(0, 2f);

        // 紅條、白條緊跟其後
        fill.DOFillAmount(targetHpPercent, 2f);
        follow.DOFillAmount(targetHpPercent, 2f);
    }
}
