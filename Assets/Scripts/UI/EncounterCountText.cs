using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EncounterCountText : MonoBehaviour
{
    private TextMeshProUGUI encounterText;
    private EncounterManager encounterManager;

    private void Start()
    {
        encounterText = GetComponentInChildren<TextMeshProUGUI>();
        encounterManager = EncounterManager.Instance;
    }

    private void Update()
    {
        encounterText.text = encounterManager.encounterCount.ToString();
    }
}
