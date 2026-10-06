using TMPro;
using UnityEngine;

public class SaveSlotItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI saveName;
    [SerializeField] private TextMeshProUGUI saveTimeAndLevel;
    [SerializeField] private TextMeshProUGUI totalPlayingTime;

    public void UpdateUIText(SaveData saveData)
    {
        
        saveName.text = $"<color=#6B75B0>Save Slot {saveData.slotIndex}</color>";
        totalPlayingTime.text = $"Playing time {saveData.formattedTotalPlayingTime}";
        saveTimeAndLevel.text = $"{saveData.saveTime}    Level: {saveData.level}";
    }
}
