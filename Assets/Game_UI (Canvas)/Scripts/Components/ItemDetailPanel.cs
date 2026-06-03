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

    private RectTransform rectTransform;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);  // ¹w³]Ãö³¬
    }
    public void ShowInfo(InventoryItemData data, Vector3 slotPosition)
    {
        if (data ==  null) return;

        gameObject.SetActive(true);

        nameText.text = data.itemName;
        descriptionText.text = data.description;
        if (iconImage != null) iconImage.sprite = data.icon;

        Vector2 targetPos = slotPosition + new Vector3(80f, 80f, 0f);
        transform.position = targetPos; 
    }
    public void HideInfo()
    {
        gameObject.SetActive(false);
    }
}
