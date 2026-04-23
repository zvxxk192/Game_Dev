using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UnityEngine.Object.FindFirstObjectByType<UIManager>();
            }
            return _instance;
        }
    }

    [Header("已註冊的 UI Root 節點")]
    [SerializeField] private PauseMenuRoot _pauseMenu;
    // 之後可加上inventoryMenu, hudMenu

    // 紀錄目前開啟中的 UI 堆疊
    private Stack<BaseView> _uiStack = new Stack<BaseView>();

    void Awake()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);
    }
    void Start()
    {
        // 讓 UIManager 去聽全域遊戲狀態
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged += HandleGameState;
        }
    }

    void HandleGameState(GameState state)
    {
        if (state == GameState.Paused)
        {
            OpenPanel(_pauseMenu);
        }
        else
        {
            CloseAllPanels();
        }
    }
    public void OpenPanel(BaseView viewToOpen)
    {
        if (viewToOpen == null) return;

        // 如果堆頂已經是此 UI 則不重複開啟
        if (_uiStack.Count > 0 && _uiStack.Peek() == viewToOpen) return;

        viewToOpen.Show();
        _uiStack.Push(viewToOpen);
    }
    public bool TryCloseTopPanel()
    {
        if (_uiStack.Count > 0)
        {
            var topView = _uiStack.Pop();
            topView.Hide();

            // 如果堆疊已空則恢復遊戲
            if (_uiStack.Count == 0 &&
                GameStateManager.Instance.CurrentState == GameState.Paused)
            {
                GameStateManager.Instance.ChangeState(GameState.Playing);
            }
            return true;
        }

        // 理論上不會觸發，為避免誤觸、防治bug
        return false;
    }
    public void CloseAllPanels()
    {
        while (_uiStack.Count > 0)
        {
            TryCloseTopPanel();
        }
    }
}
