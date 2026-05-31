using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuRouter : MonoBehaviour
{
    [System.Serializable]
    public struct TabData
    {
        public Button tabButton;             
        public BaseUISequenceView contentView;
    }

    [Header("Route Settings")]
    [SerializeField] private TabData[] tabs;

    [Header("Indicator Settings")]
    [SerializeField] private RectTransform selectionIndicator;
    [SerializeField] private float indicatorDuration = 0.3f;

    [Header("Animation Settings")]
    [SerializeField] private float btnHoverDuration = 0.2f;
    public float BtnHoverDuration
    {
        get => btnHoverDuration;
    }

    [Header("Custom Color Settings")]
    [SerializeField] private Color hoverColor = new Color(1f, 1f, 1f, 0.8f);
    public Color HoverColor
    {
        get => hoverColor;
    } 
    [SerializeField] private Color unhoverColor = new Color(0.28f, 0.28f, 0.28f);
    public Color UnhoverColor
    {
        get => unhoverColor;
    }
    [SerializeField] private Color activeColor = new Color(1f, 0.92f, 0.62f);
    public Color ActiveColor
    {
        get => activeColor;
    }

    private BaseUISequenceView currentOpenPanel;
    private int _oldIndex;

    private void Start()
    {
        // 綁定按鈕與初始化狀態
        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i;  // 解決閉包陷阱
            tabs[i].tabButton.onClick.AddListener(() => SwitchTab(index));
            

            // 初始化時，強制隱藏所有面板
            tabs[i].contentView.gameObject.SetActive(false);
        }

        // 預防讀到還未佈局完的物件位置
        Canvas.ForceUpdateCanvases();
        // 預設打開第一個分頁
        if (tabs.Length > 0)
        {
            SwitchTab(0);
        }
    }

    public void SwitchTab(int newIndex)
    {
        BaseUISequenceView targetPanel = tabs[newIndex].contentView;

        if (targetPanel == currentOpenPanel) return;

        if (selectionIndicator != null)
        {
            // 抓取玩家點擊的那個按鈕的 Y 座標
            float targetY = tabs[newIndex].tabButton.GetComponent<RectTransform>().anchoredPosition.y;

            // 殺掉游標身上可能還沒跑完的舊動畫，然後滑向新目標
            selectionIndicator?.DOKill();
            selectionIndicator.DOAnchorPosY(targetY, indicatorDuration).SetEase(Ease.OutBack).SetUpdate(true);
        }

        // 關閉舊面板
        if (currentOpenPanel != null) currentOpenPanel.ClosePanel();
        tabs[_oldIndex].tabButton.GetComponentInChildren<ChangeTMPStyle>().BtnInactive();

        // 打開新面板並更新資料
        targetPanel.OpenPanel();
        tabs[newIndex].tabButton.GetComponentInChildren<ChangeTMPStyle>().BtnActive();
        currentOpenPanel = targetPanel;
        _oldIndex = newIndex;

        // 實現第一個 (Consume) 的功能
        //if (newIndex == 0)
        //    GameStateManager.Instance.ChangeState(GameStateManager.Instance.GamePlayingState);

        // 實現最後一個 (Quit) 的功能
        // Application.Quit();
    }
}
