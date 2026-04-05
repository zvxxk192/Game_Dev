using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Data Sources")]
    public PlayerData playerData;

    public PlayerEventsManager events;

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
            events.TriggerPlayerLevelUp(currentLevel);
        }
    }
    private int currentGold = 0;
    public int CurrentGold
    {
        get => currentGold;
        set
        {
            if (value < 0) value = 0;
            currentGold = value; 
            events.TriggerPlayerGoldChanged(CurrentGold);
        }
    }
    private int currentExp = 0;
    public int CurrentExp
    {
        get => currentExp;
        set
        {
            if (value < 0) value = 0;
            currentExp = value;
            events.TriggerPlayerExpChanged(currentExp, ExpToNextLevel);
        }
    }
    private float currentHp = 100f;
    public float CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = Mathf.Clamp(value, 0, MaxHp);
            events.TriggerPlayerHpChanged(currentHp, MaxHp);
        }
    }


    [Header("Health & Survival")]
    public float MaxHp
    {
        get
        {
            float multiplier = playerData.HealthScaleCurve.Evaluate(CurrentLevel);
            return playerData.BaseMaxHp * multiplier;
        }
    }

    [Header("Movement Stats")]
    public float WalkSpeed
    {
        get
        {
            return playerData.BaseWalkSpeed;
        }
    }
    public float RunSpeed
    {
        get
        {
            return playerData.BaseRunSpeed;
        }
    }

    [Header("Leveling Cost")]
    public int ExpToNextLevel
    {
        get
        {
            float multiplier = playerData.ExpScaleCurve.Evaluate(CurrentLevel);
            return Mathf.RoundToInt(playerData.BaseExpToNextLevel * multiplier);
        }
    }

    void Awake()
    {
        CurrentHp = MaxHp;
    }
}
