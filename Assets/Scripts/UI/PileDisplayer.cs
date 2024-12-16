using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileDisplayer : MonoBehaviour
{
    public ObjectPooler cardPool;
    public Deck deck;
    public GameObject pileDisplay;

    private void Start()
    {
        cardPool.poolSize = deck.deckPile.Count;
    }

    public void ActivateAndDisplayPile(bool isDisplayingDeck)
    {
        pileDisplay.SetActive(true);
        DisplayCardPile(isDisplayingDeck);
    }

    public void DisplayCardPile(bool isDisplayingDeck)
    {
        ClearPileDisplay();

        List<Card> selectedPile = isDisplayingDeck? deck.deckPile : deck.discardPile;
        for(int i = 0; i < selectedPile.Count; i++)
        {
            GameObject displayedCard = cardPool.GetPooledObject();
            if (displayedCard == null)
                return;
            displayedCard.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            displayedCard.SetActive(true);
            displayedCard.GetComponent<CardMovementAttemp>().enabled = false;
            displayedCard.GetComponent<BoxCollider2D>().enabled = false;
            displayedCard.GetComponent<CardHover>().isNotCardDisplay = false;
            var currentCard = displayedCard.GetComponent<Card>();
            currentCard.SetUp(selectedPile[i].cardData);
        }

    }

    public void ClearPileDisplay()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
