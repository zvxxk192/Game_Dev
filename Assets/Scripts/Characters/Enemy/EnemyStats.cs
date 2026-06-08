using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Data Sources")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private PlayerStats playerStats;

    private EnemyEventsManager events;

    void Awake()
    {
        events = GetComponent<EnemyEventsManager>();
    }

    // SaveSystem.Loading()
    [Header("Enemy Stats")]
    public int CurrentLevel
    {
        get
        {
            if (playerStats.CurrentLevel > 5)
            {
                return playerStats.CurrentLevel;
            }
            return 1;
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
            float multiplier = enemyData.HealthScaleCurve.Evaluate(CurrentLevel);
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
            float multiplier = enemyData.DamageScaleCurve.Evaluate(CurrentLevel);
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
            float multiplier = enemyData.PoiseScaleCurve.Evaluate(CurrentLevel);
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

    [Header("Loot Stats")]
    public int LootGoldValue
    {
        get
        {
            float multiplier = enemyData.LootGoldScaleCurve.Evaluate(CurrentLevel);
            return (int)(enemyData.BaseLootGoldValue * multiplier);
        }
    }
    public int LootExpValue
    {
        get
        {
            float multiplier = enemyData.LootExpScaleCurve.Evaluate(CurrentLevel);
            return (int)(enemyData.BaseLootExpValue * multiplier);
        }
    }
}
