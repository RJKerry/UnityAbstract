using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Dropdown ResolutionDropdown;
    public Button ResolutionApplyButton;
    public Toggle FullscreenToggle, vSyncToggle;

    //Resolution[] resolutions;
    List<Resolution> Resolutions;
    public int currentResolutionIndex;

    private float CurrentRefreshRate;

    private void Awake()
    {
        UpdateResolutions();

        CurrentRefreshRate = Screen.currentResolution.refreshRate;

        if(ResolutionApplyButton != null)
            ResolutionApplyButton.onClick.AddListener(ApplySettings);
    }

    void UpdateResolutions()
    {
        //resolutions = Screen.resolutions;
        Resolutions = new List<Resolution>();

        foreach (Resolution resolution in Screen.resolutions)
        {
            if (resolution.refreshRate == CurrentRefreshRate)
            { 
                Resolutions.Add(resolution);
            }
        }
        List<string> options = new List<string>();

        foreach (Resolution resolution in Resolutions)
        {
            string resOpt = resolution.width + "x" + resolution.height + " " + resolution.refreshRate + " Hz";
            options.Add(resOpt);
            if (resolution.width == Screen.width && resolution.height == Screen.height)
            { currentResolutionIndex = Resolutions.IndexOf(resolution); }
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    void ApplySettings()
    {
        Screen.fullScreen = FullscreenToggle.isOn;
        QualitySettings.vSyncCount = vSyncToggle.isOn ? 1 : 0;
    }

    void SetResolution(int resolutionIndex)
    {
        Resolution resolution = Resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, FullscreenToggle.isOn);
    }
}