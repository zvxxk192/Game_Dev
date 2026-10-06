using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadSlotController : MonoBehaviour
{
    [Header("Scene to Be Load")]
    [SerializeField] private string sceneName = "Scene_PlayingWorld";

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
        for(int i = 0; i < saveSlots.Count; i++)
        {
            int index = i + 1;
            Button saveBtn = saveSlots[i].GetComponent<Button>();
            saveBtn.onClick.AddListener(() => OnClickLoadSlot(index));
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < saveSlots.Count; i++)
        {
            int index = i + 1;
            Button saveBtn = saveSlots[i].GetComponent<Button>();
            saveBtn.onClick.RemoveListener(() => OnClickLoadSlot(index));
        }
    }


    #region Btn Listener Event

    private void OnClickLoadSlot(int slotIndex)
    {
        if (SaveSystem.LoadData(slotIndex))
            GameEvents.OnRequestSceneLoad(sceneName);
    }

    #endregion
}
