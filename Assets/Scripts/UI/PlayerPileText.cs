using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPileText : MonoBehaviour
{
    private TurnSystem turn;
    private TextMeshProUGUI cardPileText;
    public bool isDisplayingDeckCount = true;

    private void Start()
    {
        turn = TurnSystem.Instance;
        cardPileText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        DisplayPileText();
    }

    private void DisplayPileText()
    {
        if (isDisplayingDeckCount)
        {
            cardPileText.text = turn.playerDeck.deckPile.Count.ToString();
        }
        else
        {
            cardPileText.text = turn.playerDeck.discardPile.Count.ToString();
        }
    }
}
