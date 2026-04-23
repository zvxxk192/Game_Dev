using System;
using UnityEngine.UIElements;

public class PauseNav : BaseView
{
    public event Action<PausePageType> OnNavClicked;

    private Button _resumeBtn;
    private Button _quitBtn;
    private Button _settingBtn;
    private Button _saveBtn;

    protected override void OnBindElements()
    {
        _resumeBtn = Root.Q<Button>("resume-btn");
        _quitBtn = Root.Q<Button>("quit-btn");
        _settingBtn = Root.Q<Button>("setting-btn");
        _saveBtn = Root.Q<Button>("save-btn");
    }
    protected override void OnRegisterEvents()
    {
        if (_resumeBtn != null) _resumeBtn.clicked += () => OnNavClicked?.Invoke(PausePageType.Enable);
        if (_quitBtn != null) _quitBtn.clicked += () => OnNavClicked?.Invoke(PausePageType.Disable);
        if (_settingBtn != null) _settingBtn.clicked += () => OnNavClicked?.Invoke(PausePageType.Settings);
        if (_saveBtn != null) _saveBtn.clicked += () => OnNavClicked?.Invoke(PausePageType.Save);
    }
    protected override void OnUnregisterEvents()
    {
        if (_resumeBtn != null) _resumeBtn.clicked -= () => OnNavClicked?.Invoke(PausePageType.Enable);
        if (_quitBtn != null) _quitBtn.clicked -= () => OnNavClicked?.Invoke(PausePageType.Disable);
        if (_settingBtn != null) _settingBtn.clicked -= () => OnNavClicked?.Invoke(PausePageType.Settings);
        if (_saveBtn != null) _saveBtn.clicked -= () => OnNavClicked?.Invoke(PausePageType.Save);
    }
}
