using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    // 存檔資料
    public int slotIndex;
    public string saveTime;
    public float totalPlayingTime;
    public string formattedTotalPlayingTime;

    // 核心數據
    public int level;
    public int exp;
    public float hp;
}
