using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemData", menuName = "Scriptable Objects/Inventory/Item Data")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public int maxStackSize = 99;
}
