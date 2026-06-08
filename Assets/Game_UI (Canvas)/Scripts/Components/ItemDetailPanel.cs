using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailPanel : MonoBehaviour
{
    public static ItemDetailPanel Instance { get; private set; }

    [Header("UI Element")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image iconImage;

    [Header("Position Settings")]
    [SerializeField] private RectTransform parentRect;
    [SerializeField] private Vector3 offset;

    private RectTransform rectTransform;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);  // 預設關閉
    }
    public void ShowInfo(InventoryItemData data, Vector3 targetWorldPosition)
    {
        if (data ==  null) return;

        nameText.text = data.itemName;
        descriptionText.text = data.description;
        if (iconImage != null) iconImage.sprite = data.icon;

        Vector3 localPos = parentRect.InverseTransformPoint(targetWorldPosition);
        rectTransform.localPosition = localPos + offset;

        // 因為有 ContentSizeFitter，需要先刷新 Layout 佈局
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        // 再動態決定 Pivot 以保證不超過螢幕
        float screenHalfWidth = Screen.width / 2f;
        float screenHalfHeight = Screen.height / 2f;
        float pivotX = (targetWorldPosition.x > screenHalfWidth) ? 1f : 0f;
        float pivotY = (targetWorldPosition.y > screenHalfHeight) ? 1f : 0f;
        rectTransform.pivot = new Vector2(pivotX, pivotY);

        gameObject.SetActive(true);
    }
    public void HideInfo()
    {
        gameObject.SetActive(false);
    }
}
