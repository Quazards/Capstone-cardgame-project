using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAreaManager : MonoBehaviour
{
    public List<Card> cardsInKeepArea = new List<Card>();
    private GameObject hand;
    private TurnSystem turn;

    private void Start()
    {
        hand = GameObject.FindGameObjectWithTag("Hand");
        turn = TurnSystem.Instance;
    }

    private void AddCardsToList()
    {
        Card[] cards = gameObject.GetComponentsInChildren<Card>();
        cardsInKeepArea.AddRange(cards);
    }

    public void ReturnCardsToHand()
    {
        AddCardsToList();

        List<Card> cardsToReturn = new List<Card>();

        foreach (Card card in cardsInKeepArea)
        {
            cardsToReturn.Add(card);
        }

        foreach (Card card in cardsToReturn)
        {
            var cardClick = card.GetComponent<CardClick>();
            cardClick.isCardSelected = false;
            card.transform.SetParent(hand.transform, false);
            cardsInKeepArea.Remove(card);
        }

        cardsInKeepArea.Clear();
    }

    public void ResetKeepStatus()
    {
        AddCardsToList();

        foreach (Card card in cardsInKeepArea)
        {
            var cardClick = card.GetComponent<CardClick>();
            cardClick.DeselectCard();
        }
        turn.ResetKeptCards();

        cardsInKeepArea.Clear();
    }
}
