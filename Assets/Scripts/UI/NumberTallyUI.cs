using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberTallyUI : MonoBehaviour
{
    private TextMeshProUGUI tallyText;
    private CombatManager combatManager;
    private PlayAreaManager playArea;
    public bool displayTotalShield = true;

    private void Start()
    {
        tallyText = GetComponentInChildren<TextMeshProUGUI>();
        combatManager = CombatManager.Instance;
        playArea = PlayAreaManager.Instance;
    }

    private void Update()
    {

        if(displayTotalShield)
        {
            tallyText.text = combatManager.playerShield.ToString() +":"+ combatManager.enemyShield.ToString();
        }
        else
        {
            tallyText.text = combatManager.playerTotalAttack.ToString() +":"+ combatManager.enemyTotalAttack.ToString();
        }
    }
}
