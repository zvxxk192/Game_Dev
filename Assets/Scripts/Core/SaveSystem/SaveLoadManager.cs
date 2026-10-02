using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    //private void Start()
    //{
    //    playerStats = GameManager.Instance.gameContext.PlayerStats;
    //}
    public void OnClickSaveSlot(int slotIndex)
    {
        SaveData data = playerStats.ExportSaveData();
        SaveSystem.Save(slotIndex, data);
    }

    public void OnClickLoadSlot(int slotIndex)
    {
        if (SaveSystem.SaveExists(slotIndex))
        {
            SaveData data = SaveSystem.Load(slotIndex);
            playerStats.ImportSaveData(data);
        }
        else
        {
            Debug.Log($"槽位 {slotIndex} 為空，無法載入！");
        }
    }
}
