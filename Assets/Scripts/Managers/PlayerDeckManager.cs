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

    private void Start()
    {
        if (playerDeck.currentDeck == null)
        {
            playerDeck.currentDeck = starterDeck;
        }
    }

    public void PlayerStartEncounter()
    {
        playerDeck.InstantiateDeck();

        playerDeck.drawAmount += startEncounterDraw;

        playerDeck.TurnStartDraw();

        playerDeck.drawAmount -= startEncounterDraw;
    }

    public void AddCardToDeck(ScriptableCard card)
    {
        playerDeck.currentDeck.addCardToPile(card);
        Debug.Log($"Added {card.card_Name} to player deck.");
    }

    public void RemoveCardFromDeck(ScriptableCard card)
    {
        playerDeck.currentDeck.removeCardFromPile(card);
        Debug.Log($"Removed {card.card_Name} from player deck.");
    }
}
