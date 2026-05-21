using UnityEngine.UIElements;
using UnityEngine;

public class HealthUI : BaseView
{
    public PlayerEventsManager events;

    private VisualElement healthFill;
    private VisualElement healthBuffer;

    protected override void OnBindElements()
    {
        healthFill = Root.Q<VisualElement>("health-fill");
        healthBuffer = Root.Q<VisualElement>("health-buffer");
    }
    protected override void OnRegisterEvents()
    {
        if (events != null) events.OnPlayerHpChanged += UpdateHealthBar;
    }
    protected override void OnUnregisterEvents()
    {
        if (events != null) events.OnPlayerHpChanged -= UpdateHealthBar;
    }

    void UpdateHealthBar(float currentHp, float maxHp)
    {
        float hpPercent = (currentHp / maxHp) * 100f;
        healthFill.style.width = new Length(hpPercent, LengthUnit.Percent);
        healthBuffer.style.width = new Length(hpPercent, LengthUnit.Percent);
    }
}
