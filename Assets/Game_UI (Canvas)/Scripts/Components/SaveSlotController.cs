using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SaveSlotController : MonoBehaviour
{
    [SerializeField] private GameObject saveSlotPrefabs = null;
    [SerializeField] private GameObject saveSlotEmptyPrefabs = null;

    private List<SaveData> saveDatas;
    private List<GameObject> saveSlots;
    private void Awake()
    {
        saveDatas = new List<SaveData>();
        saveSlots = new List<GameObject>();

        // ªì©l¤Æ Save ­¶­± UI
        saveDatas = SaveSystem.GetAllSaveDatas();

        for (int i = 0; i < saveDatas.Count; i++)
        {
            if (saveDatas[i] != null)
            {
                GameObject instance = Instantiate(saveSlotPrefabs, transform);
                SaveSlotItem slotItem = instance.GetComponent<SaveSlotItem>();
                slotItem.UpdateUIText(saveDatas[i]);
                instance.name = $"SaveSlot_{i + 1}";
                saveSlots.Add(instance);
            }
            else
            {
                GameObject instance = Instantiate(saveSlotEmptyPrefabs, transform);
                instance.name = $"SaveSlot_{i + 1}";
                saveSlots.Add(instance);
            }
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < saveSlots.Count; i++)
        {
            int index = i + 1;
            Button saveBtn = saveSlots[i].GetComponent<Button>();
            saveBtn.onClick.AddListener(() => OnClickSaveSlot(index));
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < saveSlots.Count; i++)
        {
            int index = i + 1;
            Button saveBtn = saveSlots[i].GetComponent<Button>();
            saveBtn.onClick.RemoveListener(() => OnClickSaveSlot(index));
        }
    }


    #region Btn Listener Event

    private void OnClickSaveSlot(int slotIndex)
    {
        PlayerStats playerStats = GameManager.Instance.GameContext.PlayerStats;
        SaveData newSaveData = SaveSystem.SaveData(slotIndex, playerStats.ExportSaveData());
        SaveSlotItem slotItem = saveSlots[slotIndex-1].GetComponent<SaveSlotItem>();
        slotItem.UpdateUIText(newSaveData);
    }

    #endregion
}

