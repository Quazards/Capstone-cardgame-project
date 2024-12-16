using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScalingManager : MonoBehaviour
{
    public static EnemyScalingManager Instance;

    private TurnSystem turn;
    private EncounterManager encounterManager;
    public int enemyScaling = 0;
    public bool hasScaled = false;
    public List<Card> cardsToScale = new List<Card>();

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
        encounterManager = EncounterManager.Instance;
        turn = TurnSystem.Instance;
    }

    private void Update()
    {
        ApplyScaling();
    }

    private void ApplyScaling()
    {
        if (!hasScaled)
        {
            AddCardsToList();
            ResetScaledStats();
            foreach (Card card in cardsToScale)
            {
                card.tempFrontNumber = card.cardData.front_Number + (encounterManager.loopCount);
                card.tempBackNumber = card.cardData.back_Number + (encounterManager.loopCount);
            }
            hasScaled = true;
        }
    }

    private void ResetScaledStats()
    {
        foreach(Card card in cardsToScale)
        {
            card.tempFrontNumber = card.cardData.front_Number;
            card.tempBackNumber = card.cardData.back_Number;
        }
    }

    private void AddCardsToList()
    {
        cardsToScale.Clear();

        foreach (Card card in turn.enemyDeck.deckPile)
        {
            cardsToScale.Add(card);
        }
        foreach (Card card in turn.enemyDeck.handPile)
        {
            cardsToScale.Add(card);
        }
    }
}
