using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Data Sources")]
    public EnemyData enemyData;

    private EnemyEventsManager events;

    void Awake()
    {
        events = GetComponent<EnemyEventsManager>();
    }

    // SaveSystem.Loading()
    [Header("Loading Value")]
    private int currentLevel = 1;
    public int CurrentLevel
    {
        get => currentLevel;
        set
        {
            if (value < 0) value = 0;
            currentLevel = value;
            events.TriggerEnemyLevelUp(currentLevel);
        }
    }
    private float currentHp = 100f;
    public float CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = Mathf.Clamp(value, 0, MaxHp);
            events.TriggerEnemyHpChanged(currentHp, MaxHp);
        }
    }
    private float currentPoise = 100f;
    public float CurrentPoise
    {
        get => currentPoise;
        set
        {
            currentPoise = Mathf.Clamp(value, 0, MaxPoise);
            events.TriggerEnemyPoiseChanged(currentPoise, MaxPoise);
        }
    }


    [Header("Health & Survival")]
    public float MaxHp
    {
        get
        {
            float multiplier = enemyData.HealthScaleCurve.Evaluate(currentLevel);
            return enemyData.BaseMaxHp * multiplier;
        }
    }

    [Header("Movement Stats")]
    public float PatrolSpeed
    {
        get
        {
            return enemyData.BasePatrolSpeed;
        }
    }
    public float ChaseSpeed
    {
        get
        {
            return enemyData.BaseChaseSpeed;
        }
    }

    [Header("Attack Stats")]
    public float Damage
    {
        get
        {
            float multiplier = enemyData.DamageScaleCurve.Evaluate(currentLevel);
            return enemyData.BaseDamage * multiplier;
        }
    }
    public float AttackCooldown
    {
        get
        {
            return enemyData.AttackCooldown;
        }
    }

    [Header("Defense Stats")]
    public float KnockbackForce
    {
        get
        {
            return enemyData.BaseKnockbackForce;
        }
    }
    public float MaxPoise
    {
        get
        {
            float multiplier = enemyData.PoiseScaleCurve.Evaluate(currentLevel);
            return enemyData.BaseMaxPoise * multiplier;
        }
    }
    public float PoiseResetTime
    {
        get
        {
            return enemyData.BasePoiseResetTime;
        }
    }
    public float StaggerTime
    {
        get
        {
            return enemyData.BaseStaggerTime;
        }
    }
}
