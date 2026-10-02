using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    // 存檔資料
    public int slotIndex;
    public string saveTimeStamp;

    // 核心數據
    public int level;
    public int exp;
    public float hp;

    // 玩家位置
    public float[] position = new float[3];

    public void SetPosition(Vector3 pos)
    {
        position[0] = pos.x;
        position[1] = pos.y;
        position[2] = pos.z;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(position[0], position[1], position[2]);
    }
}
