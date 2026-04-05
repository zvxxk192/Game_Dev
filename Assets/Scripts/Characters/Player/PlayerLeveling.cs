using UnityEngine;
using System;

public class PlayerLeveling : MonoBehaviour
{
    private PlayerStats stats;

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }
    public void AddExp(int amount)
    {
        stats.CurrentExp += amount;
        Debug.Log($"獲得經驗: {amount}");
        while(stats.CurrentExp > stats.ExpToNextLevel)
        {
            LevelUp();
        }
    }
    void LevelUp()
    {
        stats.CurrentExp -= stats.ExpToNextLevel;
        stats.CurrentLevel++;
        Debug.Log($"升級了! 目前等級: {stats.CurrentLevel}");
    }
}
