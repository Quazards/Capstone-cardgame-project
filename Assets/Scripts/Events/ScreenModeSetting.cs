using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenModeSetting : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown ScreenModeDropDown;
    private Resolutions resolutions;

    void Start()
    {
        resolutions = Object.FindFirstObjectByType<Resolutions>();

        if (resolutions == null)
            return;

        int val = PlayerPrefs.GetInt("ScreenMode", 0);
        ScreenModeDropDown.value = val;

        ScreenModeDropDown.RefreshShownValue();
        SetScreenMode(val);
    }

    public void SetScreenMode(int index)
    {
        PlayerPrefs.SetInt("ScreenMode", index);


        if (index == 0 && resolutions != null)
        {
            Screen.SetResolution(resolutions.CurrentResolution.width, resolutions.CurrentResolution.height, true);
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else if (index == 1 && resolutions != null)
        {
            Screen.SetResolution(resolutions.CurrentResolution.width, resolutions.CurrentResolution.height, true);
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if (index == 2 && resolutions != null)
        {
            Screen.SetResolution(resolutions.CurrentResolution.width, resolutions.CurrentResolution.height, false);
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
}