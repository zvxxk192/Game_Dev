using UnityEngine;
using System;

public class PlayerWallet : MonoBehaviour
{
    private PlayerStats stats;

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }
    public void AddGold(int amount)
    {
        stats.CurrentGold += amount;
        Debug.Log($"{name} 獲得錢幣 ! 目前金幣數量 : {stats.CurrentGold}");
    }
    public bool SpendGold(int amount)
    {
        if(stats.CurrentGold > amount )
        {
            stats.CurrentGold -= amount;
            return true;
        }
        Debug.Log("金幣不足!");
        return false;
    }
}
