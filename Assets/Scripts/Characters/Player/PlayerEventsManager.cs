using UnityEngine;
using System;

public class PlayerEventsManager : MonoBehaviour
{
    public event Action<float, float> OnPlayerHpChanged;
    public event Action<float, float> OnPlayerExpChanged;
    public event Action<int> OnPlayerGoldChanged;
    public event Action<int> OnPlayerLevelUp;

    public void TriggerPlayerHpChanged(float currentHp, float maxHp)
        => OnPlayerHpChanged?.Invoke(currentHp, maxHp);
    public void TriggerPlayerExpChanged(float currentExp, float maxExp)
        => OnPlayerExpChanged?.Invoke(currentExp, maxExp);
    public void TriggerPlayerGoldChanged(int currentGold)
        => OnPlayerGoldChanged?.Invoke(currentGold);
    public void TriggerPlayerLevelUp(int currentLevel)
        => OnPlayerLevelUp?.Invoke(currentLevel);

    // ---------- VFX ---------------
    public event Action OnPerfectDodge;

    public void TriggerPerfectDodge()
    {
        OnPerfectDodge?.Invoke();
    }
}
