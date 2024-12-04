using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    string scoreKey = "Score";

    public int CurrentScore { get; set; }

    private void Awake()
    {
        CurrentScore = PlayerPrefs.GetInt(scoreKey);
    }

    public void SetHealth(int score)
    {
        PlayerPrefs.SetInt(scoreKey, score);
    }
}
