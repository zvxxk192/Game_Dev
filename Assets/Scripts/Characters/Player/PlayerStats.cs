using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Data Sources")]
    public PlayerData playerData;

    public PlayerEventsManager events;

    [Header("Loading Value")]
    private int currentLevel = 1;
    public int CurrentLevel
    {
        get => currentLevel;
        set
        {
            if (value < 0) value = 1;
            currentLevel = value;
            events.TriggerPlayerLevelUp(currentLevel);
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
            //if (currentExp >= ExpToNextLevel)
            //{
            //    currentExp -= ExpToNextLevel;
            //    CurrentLevel++;
            //}
            events.TriggerPlayerExpChanged(currentExp, ExpToNextLevel);
        }
    }
    private float currentHp = 100f;
    public float CurrentHp
    {
        get => currentHp;
        set
        {
            if (value == currentHp) return;
            bool isIncrease = value > currentHp;
            currentHp = Mathf.Clamp(value, 0, MaxHp);
            events.TriggerPlayerHpChanged(currentHp, MaxHp, isIncrease);
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


    #region Save Syste

    public SaveData ExportSaveData()
    {
        SaveData data = new SaveData();
        data.level = CurrentLevel;
        data.exp = CurrentExp;
        data.hp = CurrentHp;

        data.SetPosition(transform.position);

        return data;
    }

    public void ImportSaveData(SaveData data)
    {
        if (data == null) return;

        CurrentLevel = data.level;
        CurrentExp = data.exp;
        CurrentHp = data.hp;

        transform.position = data.GetPosition();

        Debug.Log("[PlayerStats] 玩家屬性已成功載入並更新！");
    }

    #endregion
}
