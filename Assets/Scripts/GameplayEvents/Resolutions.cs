using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolutions : MonoBehaviour, IDataPersistence
{
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropDown; 
    
    private Resolution[] resolutions = new Resolution[]
    {
        new Resolution { width = 2560, height = 1440 },
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1600, height = 900 }
    };
    
    private List<Resolution> filteredResolutions;

    private int currentResolutionIndex = 0;
    
    public Resolution CurrentResolution => filteredResolutions[currentResolutionIndex];

    private void Awake()
    {
        filteredResolutions = new List<Resolution>(resolutions);
    }

    private void Start()
    {
        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + " x " + filteredResolutions[i].height;
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        currentResolutionIndex = resolutionIndex;
        int screenMode = PlayerPrefs.GetInt("ScreenMode", 0);
        
        Screen.SetResolution(CurrentResolution.width, CurrentResolution.height, screenMode != 2);
        SetFullScreenMode(screenMode);
    }

    private void SetFullScreenMode(int mode)
    {
        switch (mode)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.CurrentResolution = this.CurrentResolution;
        Debug.Log($"Saving Resolution: {data.CurrentResolution.width} x {data.CurrentResolution.height}");
    }

    public void LoadData(GameData data)
    {
        currentResolutionIndex = filteredResolutions.FindIndex(res =>
            res.width == data.CurrentResolution.width && res.height == data.CurrentResolution.height);

        if (currentResolutionIndex == -1)
        {
            currentResolutionIndex = 0;
            Debug.LogWarning("Saved resolution not found. Defaulting to the first resolution.");
        }

        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();

        SetResolution(currentResolutionIndex);
    }
}