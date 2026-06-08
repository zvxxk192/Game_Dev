using UnityEngine;
using System;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField] private PlayerEventsManager events;
    [SerializeField] private PlayerStats stats;

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

    void Awake()
    {
        events = GetComponent<PlayerEventsManager>();
        stats = GetComponent<PlayerStats>();
    }
    public void AddGold(int amount)
    {
        currentGold += amount;
        Debug.Log($"{name} 獲得錢幣 ! 目前金幣數量 : {currentGold}");
    }
    public bool SpendGold(int amount)
    {
        if(currentGold > amount )
        {
            currentGold -= amount;
            return true;
        }
        Debug.Log("金幣不足!");
        return false;
    }
}
