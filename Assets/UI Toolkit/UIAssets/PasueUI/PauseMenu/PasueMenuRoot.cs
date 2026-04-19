using UnityEngine;
using UnityEngine.UIElements;

public class PasueMenuRoot : BaseView
{
    //private Button _resumeBtn;
    //private Button _quitBtn;

    //protected override void OnBindElements()
    //{
    //    _resumeBtn = Root.Q<Button>("resume-btn");
    //    _quitBtn = Root.Q<Button>("quit-btn");
    //}
    //protected override void OnRegisterEvents()
    //{
    //    if (GameStateManager.Instance != null)
    //        GameStateManager.Instance.OnGameStateChanged += ChangedToPausedMenu;

    //    if (_resumeBtn != null) _resumeBtn.clicked += OnResumeClicked;
    //    if (_quitBtn != null) _quitBtn.clicked += OnQuitClicked;
    //}
    //protected override void OnUnregisterEvents()
    //{
    //    if (_resumeBtn != null) _resumeBtn.clicked -= OnResumeClicked;
    //    if (_quitBtn != null) _quitBtn.clicked -= OnQuitClicked;
    //    if (GameStateManager.Instance != null)
    //        GameStateManager.Instance.OnGameStateChanged -= ChangedToPausedMenu;
    //}

    [Header("Child Component")]
    [SerializeField] private PauseNav _navComponent;
    [SerializeField] private PauseContent _contentComponent;

    protected override void OnBindElements() { }
    protected override void OnRegisterEvents()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged += ChangedToPausedMenu;
            ChangedToPausedMenu(GameStateManager.Instance.CurrentState);
        }
        if (_navComponent != null && _contentComponent != null)
        {
            _navComponent.OnNavClicked += _contentComponent.ShowPage;
        }
    }
    protected override void OnUnregisterEvents()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged -= ChangedToPausedMenu;
            ChangedToPausedMenu(GameStateManager.Instance.CurrentState);
        }
        if (_navComponent != null && _contentComponent != null)
        {
            _navComponent.OnNavClicked -= _contentComponent.ShowPage;
        }
    }
    protected override void OnPostInitialize()
    {
        Root.style.display = DisplayStyle.None;
    }
    void ChangedToPausedMenu(GameState newState)
    {
        if (newState == GameState.Paused)
        {
            Time.timeScale = 0f;
            Root.style.display = DisplayStyle.Flex;
            if (_contentComponent != null)
                _contentComponent.ShowPage(PausePageType.Enable);
        }
        else
        {
            Time.timeScale = 1.0f;
            Root.style.display = DisplayStyle.None;
        }
    }

    //void OnResumeClicked() => GameStateManager.Instance.ChangeState(GameState.Playing);
    //void OnQuitClicked() => GameStateManager.Instance.ChangeState(GameState.Paused);
}
