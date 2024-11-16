using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckManager : MonoBehaviour
{
    public static PlayerDeckManager Instance;

    public Deck playerDeck;

    public int startEncounterDraw = 2;

    [SerializeField] private CardPile starterDeck;

    [SerializeField] private CardPile tempDeck;

    public List<ScriptableCard> globalDeck = new();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void BuildStarterDeck()
    {
        globalDeck.AddRange(starterDeck.cardsInPile);

        tempDeck.cardsInPile.AddRange(globalDeck);
    }

    private void BuildDeck()
    {
        tempDeck.cardsInPile.AddRange(globalDeck);
    }

    public void PlayerStartEncounter()
    {
        if (playerDeck.currentDeck == null)
        {
            tempDeck.cardsInPile.Clear();
            playerDeck.currentDeck = tempDeck;
            BuildStarterDeck();
        }
        else
        {
            tempDeck.cardsInPile.Clear();
            BuildDeck();
        }

        playerDeck.InstantiateDeck();
        playerDeck.drawAmount += startEncounterDraw;
        playerDeck.TurnStartDraw();

        playerDeck.drawAmount -= startEncounterDraw;
    }

    public void PlayerEndEncounter()
    {
        playerDeck.ClearAll();
        tempDeck.cardsInPile.Clear();
    }

    public void AddCardToDeck(ScriptableCard card)
    {
        //playerDeck.currentDeck.addCardToPile(card);
        globalDeck.Add(card);
        Debug.Log($"Added {card.card_Name} to player deck.");
    }

    public void RemoveCardFromDeck(ScriptableCard card)
    {
        //playerDeck.currentDeck.removeCardFromPile(card);
        globalDeck.Remove(card);
        Debug.Log($"Removed {card.card_Name} from player deck.");
    }
}
