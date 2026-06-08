using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI Element")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI nameText;

    private RectTransform rectTransform;

    // ---- 讀取玩家資料 -----
    [SerializeField] private InventoryItemData currentItem;
    private int currentCount = 1;
    public InventoryItemData CurrentItem
    {
        get => currentItem;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        SetItem(currentItem, currentCount);
    }
    public void SetItem(InventoryItemData data, int count)
    {
        currentItem = data;
        currentCount = count;

        if (data != null)
        {
            iconImage.sprite = data.icon;
            iconImage.enabled = true;
            nameText.text = data.itemName;
            countText.text = count > 1 ? count.ToString() : "";
        }
        else
        {
            ClearSlot();
        }
    }
    public void ClearSlot()
    {
        currentItem = null;
        currentCount = 0;
        iconImage.enabled = false;
        countText.text = "";
    }


    // --- 滑鼠互動 ---
    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if (CurrentItem == null) return;

    //    if (eventData.button == PointerEventData.InputButton.Left)
    //    {
    //        // InventoryManager.Instance.SelectItem(this);
    //    }
    //    else if (eventData.button == PointerEventData.InputButton.Right)
    //    {

    //    }
    //}
    // 顯示訊息
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem == null) return;

        ItemDetailPanel.Instance.ShowInfo(CurrentItem, rectTransform.position);
    }
    // 隱藏訊息
    public void OnPointerExit(PointerEventData eventData)
    {
        ItemDetailPanel.Instance.HideInfo();
    }
}
