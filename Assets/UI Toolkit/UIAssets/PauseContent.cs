using UnityEngine.UIElements;

public class PauseContent : BaseView
{
    private VisualElement _settingsPage;
    private VisualElement _savePage;

    protected override void OnBindElements()
    {
        _settingsPage = Root.Q<VisualElement>("pause-page-settings");
        _savePage = Root.Q<VisualElement>("pause-page-save");
    }
    protected override void OnRegisterEvents() { }
    protected override void OnUnregisterEvents() { }
    protected override void OnPostInitialize()
    {
        ShowPage(PausePageType.Disable);
    }

    public void ShowPage(PausePageType pageType)
    {
        _settingsPage.style.display = DisplayStyle.None;
        _savePage.style.display = DisplayStyle.None;

        switch (pageType)
        {
            case PausePageType.Enable:
                GameStateManager.Instance.ChangeState(GameState.Paused);
                break;
            case PausePageType.Disable:
                GameStateManager.Instance.ChangeState(GameState.Playing);
                break;
            case PausePageType.Save:
                _savePage.style.display = DisplayStyle.Flex;
                break;
            case PausePageType.Settings:
                _settingsPage.style.display = DisplayStyle.Flex;
                break;
        }
    }
}
