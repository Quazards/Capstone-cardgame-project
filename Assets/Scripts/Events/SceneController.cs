using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    public List<string> storedSceneNames = new List<string>();
    public List<Slider> sliders;

        private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void StoreScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName) && !storedSceneNames.Contains(sceneName))
        {
            storedSceneNames.Add(sceneName);
            Debug.Log("Stored Scene: " + sceneName);
        }
        else
        {
            Debug.Log("Scene not found or already stored.");
        }
    }

    public List<string> GetStoredScenes()
    {
        return storedSceneNames;
    }

    public void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            Debug.Log("Loading Scene: " + sceneName);
        }
        
        else
        {
            Debug.LogError("Scene name is invalid.");
        }
    }

        public void QuitGame()
    {
        Debug.Log("Game is quitting...");

        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void SetAllSlidersValue(float value)
    {
        foreach (Slider slider in sliders)
        {
            if (slider != null)
            {
                slider.value = value;
            }
        }
    }
}