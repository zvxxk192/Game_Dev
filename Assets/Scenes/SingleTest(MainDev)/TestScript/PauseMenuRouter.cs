using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuRouter : MonoBehaviour
{
    [System.Serializable]
    public struct TabData
    {
        public Button tabButton;             
        public BaseUISequenceView panelView;
    }

    [Header("Route Settings")]
    [SerializeField] private TabData[] tabs;

    [Header("Indicator Settings")]
    [SerializeField] private RectTransform selectionIndicator;
    [SerializeField] private float indicatorDuration = 0.3f;

    private BaseUISequenceView currentOpenPanel;

    private void Start()
    {
        // 綁定按鈕與初始化狀態
        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i;  // 解決閉包陷阱
            tabs[i].tabButton.onClick.AddListener(() => SwitchTab(index));

            // 初始化時，強制隱藏所有面板
            tabs[i].panelView.gameObject.SetActive(false);
        }

        // 預設打開第一個分頁
        if (tabs.Length > 0)
        {
            SwitchTab(0);
        }
    }

    public void SwitchTab(int newIndex)
    {
        BaseUISequenceView targetPanel = tabs[newIndex].panelView;

        if (targetPanel == currentOpenPanel) return;

        if (selectionIndicator != null)
        {
            // 抓取玩家點擊的那個按鈕的 Y 座標
            float targetY = tabs[newIndex].tabButton.GetComponent<RectTransform>().anchoredPosition.y;

            // 殺掉游標身上可能還沒跑完的舊動畫，然後滑向新目標
            selectionIndicator.DOKill();
            selectionIndicator.DOAnchorPosY(targetY, indicatorDuration).SetEase(Ease.OutBack);
        }

        // 關閉舊面板
        if (currentOpenPanel != null)
        {
            currentOpenPanel.ClosePanel();
        }

        // 打開新面板
        targetPanel.OpenPanel();
        currentOpenPanel = targetPanel;
    }
}
