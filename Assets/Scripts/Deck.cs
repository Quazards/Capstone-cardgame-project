using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Singleton (katanya)
    public static Deck Instance { get; private set; }

    [SerializeField] private CardPile playerDeck;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Canvas cardCanvas;

    public List<Card> deckPile = new();
    public List<Card> discardPile = new();
    public List<Card> handPile { get; private set; } = new();
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InstantiateDeck(); // -> to instantiate deck at the start of a level
        TurnStartDraw();
    }

    private void InstantiateDeck()
    {
        for(int i = 0; i < playerDeck.cardsInPile.Count; i++)
        {
            Card card = Instantiate(cardPrefab, cardCanvas.transform);
            card.setUp(playerDeck.cardsInPile[i]);
            card.gameObject.SetActive(false);
            deckPile.Add(card);
        }
        Shuffle();

    }

    private void Shuffle()
    {
        for(int i = deckPile.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = deckPile[i];
            deckPile[i] = deckPile[j];
            deckPile[j] = temp;
        }
    }

    public void TurnStartDraw(int value = 4)
    {
        for(int i = 0; i < value; i++)
        {
            if(deckPile.Count <= 0)
            {
                discardPile = deckPile;
                discardPile.Clear();
                Shuffle();
            }
            handPile.Add(deckPile[0]);
            deckPile[0].gameObject.SetActive(true);
            deckPile.RemoveAt(0);

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
}
