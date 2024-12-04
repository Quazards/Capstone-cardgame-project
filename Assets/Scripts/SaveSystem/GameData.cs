using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData : ISerializationCallbackReceiver
{
    public int playerCurrentHealth;
    public Resolution CurrentResolution;
    public List<ScriptableCard> globalDeck = new();
    public bool isStarterDeckInitialized = false;
    public int encounterCount;
    public bool isFirstEncounter;

    [SerializeField] private int resolutionWidth;
    [SerializeField] private int resolutionHeight;

    public GameData()
    {
        this.playerCurrentHealth = 50;
        this.CurrentResolution = new Resolution() { width = 1920, height = 1080 };
        this.globalDeck = new List<ScriptableCard>();
        this.isStarterDeckInitialized = false;
        this.encounterCount = 0;
        this.isFirstEncounter = true;
    }

    public void OnBeforeSerialize()
    {
        resolutionWidth = CurrentResolution.width;
        resolutionHeight = CurrentResolution.height;
    }

    public void OnAfterDeserialize()
    {
        CurrentResolution = new Resolution { width = resolutionWidth, height = resolutionHeight };
    }
}