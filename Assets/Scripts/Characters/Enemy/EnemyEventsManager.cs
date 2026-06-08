using UnityEngine;
using System;

public class EnemyEventsManager : MonoBehaviour
{
    public event Action<float, float> OnEnemyHpChanged;
    public event Action<float, float> OnEnemyPoiseChanged;

    public void TriggerEnemyHpChanged(float currentHp, float maxHp)
        => OnEnemyHpChanged?.Invoke(currentHp, maxHp);
    public void TriggerEnemyPoiseChanged(float currentPoise, float maxPoise)
        => OnEnemyPoiseChanged?.Invoke(currentPoise, maxPoise);
}
