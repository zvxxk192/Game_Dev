using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string GetSavePath(int slotIndex)
    {
        // persistentDataPath 等於 AppData/LocalLow/Unity/專案名
        return Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");
    }

    public static void Save(int slotIndex, SaveData data)
    {
        data.slotIndex = slotIndex;
        data.saveTimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(GetSavePath(slotIndex), json);
        Debug.Log($"[SaveSystem] 槽位 {slotIndex} 存檔成功！路徑: {GetSavePath(slotIndex)}");
    }

    public static SaveData Load(int slotIndex)
    {
        string path = GetSavePath(slotIndex);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"[SaveSystem] 槽位 {slotIndex} 找不到存檔檔案！");
            return null;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Debug.Log($"[SaveSystem] 槽位 {slotIndex} 讀檔成功！");
        return data;
    }

    // 檢查槽位
    public static bool SaveExists(int slotIndex)
    {
        return File.Exists(GetSavePath(slotIndex));
    }

    public static void DeleteSave(int slotIndex)
    {
        string path = GetSavePath(slotIndex);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"[SaveSystem] 槽位 {slotIndex} 已刪除！");
        }
    }
}
