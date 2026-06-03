using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEditor.Rendering;

public class DisplaySettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown windowModeDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private List<Resolution> filterResolution = new List<Resolution>();

    private void Start()
    {
        SetupWindowModeDropdown();
        SetupResolutionDropdown();
    }
    private void SetupWindowModeDropdown()
    {
        windowModeDropdown.ClearOptions();

        List<string> options = new List<string>
        {
            "FullScreen",
            "Borderless",
            "Windowed"
        };
        windowModeDropdown.AddOptions(options);

        // 讓 mode 對齊 UI
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen: 
                windowModeDropdown.value = 0;
                break;
            case FullScreenMode.FullScreenWindow:
                windowModeDropdown.value = 1;
                break;
            case FullScreenMode.Windowed:
                windowModeDropdown.value = 2;
                break;
        }
        // 刷新
        windowModeDropdown.RefreshShownValue();

        // 監聽已達到響應式
        windowModeDropdown.onValueChanged.AddListener(OnWindowModeDropdownValueChanged);
    }
    private void OnWindowModeDropdownValueChanged(int value)
    {
        FullScreenMode mode = FullScreenMode.FullScreenWindow;
        switch (value)
        {
            case 0: 
                mode = FullScreenMode.ExclusiveFullScreen; 
                break;
            case 1:
                mode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                mode = FullScreenMode.Windowed;
                break;
        }
        Screen.fullScreenMode = mode;
    }
    private void SetupResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        Resolution[] allResolutions = Screen.resolutions;
        List<string> options = new List<string>();

        // 先處理系統支援哪些解析度
        for (int i = 0; i < allResolutions.Length; i++)
        {
            if (options.Contains($"{allResolutions[i].width} x {allResolutions[i].height}") == false)
            {
                filterResolution.Add(allResolutions[i]);
                options.Add($"{allResolutions[i].width} x {allResolutions[i].height}");
            } 
        }

        resolutionDropdown.AddOptions(options);

        // 尋找現在的解析度，並更新
        int currentResolutionValue = -1;
        for (int i = 0; i < filterResolution.Count; i++)
        {
            if (filterResolution[i].width == Screen.currentResolution.width &&
                filterResolution[i].height == Screen.currentResolution.height)
            {
                currentResolutionValue = i;
                break;
            }
        }
        resolutionDropdown.value = currentResolutionValue;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(OnResolutionValueChanged);
    }
    private void OnResolutionValueChanged(int value)
    {
        Resolution targetRs = filterResolution[value];
        Screen.SetResolution(targetRs.width, targetRs.height, Screen.fullScreenMode);
    }
}
