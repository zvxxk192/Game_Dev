using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public static class SaveSystem
{
    public static int SelectedSlot { get; private set; } = 0;
    public static bool IsLoadingSave { get; private set; } = false;
    public static SaveData CurrentSaveData { get; private set; } = null;

    private static string GetSavePath(int slotIndex)
    {
        // persistentDataPath 等於 AppData/LocalLow/Unity/專案名
        return Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");
    }

    public static SaveData SaveData(int slotIndex, SaveData data)
    {
        data.slotIndex = slotIndex;
        data.saveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        System.TimeSpan time = System.TimeSpan.FromSeconds(data.totalPlayingTime);
        data.formattedTotalPlayingTime = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)time.TotalHours, time.Minutes, time.Seconds);

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(GetSavePath(slotIndex), json);
        Debug.Log($"[SaveSystem] 槽位 {slotIndex} 存檔成功！路徑: {GetSavePath(slotIndex)}");

        return data;
    }

    public static bool LoadData(int slotIndex)
    {
        if (!SaveExists(slotIndex) || slotIndex == 0)
        {
            Debug.LogWarning($"[SaveSystem] 槽位 {slotIndex} 找不到存檔檔案！");
            return false;
        }

        SelectedSlot = slotIndex;
        IsLoadingSave = true;

        string path = GetSavePath(slotIndex);
        string json = File.ReadAllText(path);
        CurrentSaveData = JsonUtility.FromJson<SaveData>(json);

        Debug.Log($"[SaveSystem] 槽位 {slotIndex} 讀檔成功！");
        return true;   }

    // 檢查槽位
    private static bool SaveExists(int slotIndex)
    {
        return File.Exists(GetSavePath(slotIndex));
    }

    public static List<SaveData> GetAllSaveDatas()
    {
        List<SaveData> saveDatas = new List<SaveData>();
        for(int i = 1; i <= 30; i++)
        {
            if (SaveExists(i))
            {
                string path = GetSavePath(i);
                string json = File.ReadAllText(path);
                SaveData saveData = JsonUtility.FromJson<SaveData>(json);
                saveDatas.Add(saveData);
            }
            else
            {
                saveDatas.Add(null);
            }
        }
        return saveDatas;
    }

    //public static void DeleteSave(int slotIndex)
    //{
    //    string path = GetSavePath(slotIndex);
    //    if (File.Exists(path))
    //    {
    //        File.Delete(path);
    //        Debug.Log($"[SaveSystem] 槽位 {slotIndex} 已刪除！");
    //    }
    //}
}
 