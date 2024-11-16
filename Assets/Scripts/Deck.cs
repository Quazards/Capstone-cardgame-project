using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deck : MonoBehaviour
{
    public CardPile currentDeck;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Canvas cardCanvas;
    private TurnSystem turnSystem;

    public int drawAmount = 1;

    public List<Card> deckPile = new();
    public List<Card> discardPile = new();
    public List<Card> handPile { get; private set; } = new();


    private void Start()
    {
        turnSystem = TurnSystem.Instance;
    }

    public void ClearAll()
    {
        foreach (Card card in deckPile)
        {
            Destroy(card.gameObject);
        }

        deckPile.Clear();
        discardPile.Clear();
        handPile.Clear();
    }

    public void InstantiateDeck()
    {
        if (currentDeck == null) return;

        for(int i = 0; i < currentDeck.cardsInPile.Count; i++)
        {
            Card card = Instantiate(cardPrefab, cardCanvas.transform);
            card.setUp(currentDeck.cardsInPile[i]);
            card.gameObject.SetActive(false);
            deckPile.Add(card);
        }
        Shuffle();

    }

    public void Shuffle()
    {
        for(int i = deckPile.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = deckPile[i];
            deckPile[i] = deckPile[j];
            deckPile[j] = temp;
        }
    }

    public void TurnStartDraw()
    {
            for (int i = 0; i < drawAmount; i++)
            {
                if (deckPile.Count == 0 && discardPile.Count != 0)
                {
                    deckPile.AddRange(discardPile);
                    discardPile.Clear();
                    Shuffle();
                }

                if (deckPile.Count > 0)
                {
                    Card drawnCard = deckPile[0];
                    handPile.Add(drawnCard);
                    deckPile[0].gameObject.SetActive(true);
                    deckPile.RemoveAt(0);
                    
                    CardMovementAttemp cardMovemnt = drawnCard.GetComponent<CardMovementAttemp>();
                    if (cardMovemnt != null)
                    {
                        cardMovemnt.hasFlipped = false;
                    }
                    
                    if(drawnCard.cardData.card_Ownership == CardOwnership.Player)
                    {
                        drawnCard.transform.SetParent(GameObject.FindGameObjectWithTag("Hand").transform);
                    }
                    else if(drawnCard.cardData.card_Ownership == CardOwnership.Enemy)
                    {
                        drawnCard.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyArea").transform);
                    }
                }
            }

    }

    public void Discard(Card card)
    {
        if(handPile.Contains(card))
        {
            handPile.Remove(card);
            discardPile.Add(card);
            card.gameObject.SetActive(false);   
        }
    }

    public void AdditionalDraw()
    {
        if(TurnSystem.Instance.currentEnergy >= 1)
        {
            TurnStartDraw();
            TurnSystem.Instance.currentEnergy -= 1;
        }
        else
        {
            Debug.Log("You dont have enough energy to draw");
        }
    }
}
