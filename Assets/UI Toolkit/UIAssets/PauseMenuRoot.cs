using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuRoot : BaseView
{
    [Header("Child Component")]
    [SerializeField] private PauseNav _navComponent;
    [SerializeField] private PauseContent _contentComponent;

    protected override void OnBindElements() { }
    protected override void OnRegisterEvents()
    {
        if (_navComponent != null && _contentComponent != null)
        {
            _navComponent.OnNavClicked += _contentComponent.ShowPage;
        }
    }
    protected override void OnUnregisterEvents()
    {
        if (_navComponent != null && _contentComponent != null)
        {
            _navComponent.OnNavClicked -= _contentComponent.ShowPage;
        }
    }
    protected override void OnPostInitialize()
    {
        this.Hide();
    }

    protected override void OnShow()
    {
        Time.timeScale = 0f;
        if (_contentComponent != null)
            _contentComponent.ShowPage(PausePageType.Enable);
    }
    protected override void OnHide()
    {
        Time.timeScale = 1.0f;
    }
}
