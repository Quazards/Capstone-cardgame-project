using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckManager : MonoBehaviour, IDataPersistence
{
    public static PlayerDeckManager Instance;
    public Deck playerDeck;
    [SerializeField] private CardPile starterDeck;
    [SerializeField] private CardPile tempDeck;
    public List<ScriptableCard> globalDeck = new();
    [HideInInspector] public bool isStarterDeckInitialized = false;

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

    public void BuildStarterDeck()
    {
        if (isStarterDeckInitialized)
        {
            Debug.Log("StarterDeck already initialized. Skipping...");
            return;
        }
        globalDeck.AddRange(starterDeck.cardsInPile);
        tempDeck.cardsInPile.AddRange(globalDeck);

        isStarterDeckInitialized = true;
        Debug.Log("StarterDeck initialized for the first time.");
    }

    private void BuildDeck()
    {
        tempDeck.cardsInPile.AddRange(globalDeck);
    }

    public void PlayerStartEncounter()
    {
        if (playerDeck.currentDeck == null)
        {
            ClearTempDeck();
            playerDeck.currentDeck = tempDeck;
            BuildStarterDeck();
        }
        else
        {
            ClearTempDeck();
            BuildDeck();
        }

        playerDeck.InstantiateDeck();
        playerDeck.drawAmount = 4;
        playerDeck.TurnStartDraw();
        playerDeck.drawAmount = 1;
    }

    public void PlayerEndEncounter()
    {
        playerDeck.ClearAll();
        tempDeck.cardsInPile.Clear();
    }

    private void ClearTempDeck()
    {
        List<ScriptableCard> cardsToRemove = new List<ScriptableCard>();

        foreach(var card in tempDeck.cardsInPile)
        {
            cardsToRemove.Add(card);
        }

        foreach(var card in cardsToRemove)
        {
            tempDeck.cardsInPile.Remove(card);
        }

        tempDeck.cardsInPile.Clear();
    }

    public void AddCardToDeck(ScriptableCard card)
    {
        globalDeck.Add(card);
        Debug.Log($"Added {card.card_Name} to player deck.");
    }

    public void RemoveCardFromDeck(ScriptableCard card)
    {
        globalDeck.Remove(card);
        Debug.Log($"Removed {card.card_Name} from player deck.");
    }

    public void DeleteDeckData()
    {
        PlayerHealth.Instance.playerCurrentHealth = 50;
        this.globalDeck.Clear();
        isStarterDeckInitialized = false;
    }

    public void LoadData(GameData data)
    {
        this.globalDeck = data.globalDeck;
        this.isStarterDeckInitialized = data.isStarterDeckInitialized;
    }

    public void SaveData(ref GameData data)
    {
        data.globalDeck = this.globalDeck;
        data.isStarterDeckInitialized = this.isStarterDeckInitialized;
    }
}