using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoopCountText : MonoBehaviour
{
    private TextMeshProUGUI loopText;
    private EncounterManager encounterManager;

    private void Start()
    {
        loopText = GetComponentInChildren<TextMeshProUGUI>();
        encounterManager = EncounterManager.Instance;
    }

    private void Update()
    {
        loopText.text = encounterManager.loopCount.ToString();
    }
}
