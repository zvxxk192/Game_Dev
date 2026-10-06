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
        public ChangeTMPStyle changeTMPStyle;
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
    [SerializeField] private Color hoverColor;
    public Color HoverColor
    {
        get => hoverColor;
    } 
    [SerializeField] private Color unhoverColor;
    public Color UnhoverColor
    {
        get => unhoverColor;
    }
    [SerializeField] private Color activeColor;
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
            SwitchTab(0, true);
        }
    }

    public void SwitchTab(int newIndex, bool isFirstOpen = false)
    {
        BaseUISequenceView targetPanel = tabs[newIndex].contentView;

        if (selectionIndicator != null)
        {
            // 抓取玩家點擊的那個按鈕的 Y 座標
            float targetY = tabs[newIndex].tabButton.GetComponent<RectTransform>().anchoredPosition.y;

            // 殺掉游標沒跑完的動畫，直接滑向新目標
            selectionIndicator?.DOKill();
            selectionIndicator.DOAnchorPosY(targetY, indicatorDuration).SetEase(Ease.OutBack).SetUpdate(true);
        }

        // 關閉舊面板
        if (currentOpenPanel != null) currentOpenPanel.ClosePanel();
        tabs[_oldIndex].changeTMPStyle.BtnInactive();

        // 打開新面板並更新資料
        targetPanel.OpenPanel();
        tabs[newIndex].changeTMPStyle.BtnActive();
        currentOpenPanel = targetPanel;
        _oldIndex = newIndex;

        // 打開初始狀態(Consume)頁面，但不要實現
        if (isFirstOpen) return;

        // 實現第一個 (Consume) 的功能
        if (newIndex == 0)
        {
            GameStateManager.Instance.ChangeState(GameStateManager.Instance.GamePlayingState);
        }

        // 實現最後一個 (Quit) 的功能
        if (newIndex ==  tabs.Length - 1)
        {
            GameEvents.OnRequestSceneLoad("Scene_MainMenu");
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i;  // 解決閉包陷阱
            tabs[i].tabButton.onClick.RemoveListener(() => SwitchTab(index));
        }
    }
}
