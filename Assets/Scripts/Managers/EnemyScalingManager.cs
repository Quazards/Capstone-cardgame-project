using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScalingManager : MonoBehaviour
{
    public static EnemyScalingManager Instance;
    private TurnSystem turn;
    private DataPersistenceManager dataPersistence;
    public int enemyScaling = 0;
    public bool hasScaled = false;

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
        dataPersistence = DataPersistenceManager.instance;
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
            ResetScaledStats();
            foreach (Card card in turn.enemyDeck.handPile)
            {
                card.tempFrontNumber = card.cardData.front_Number + (dataPersistence.gameData.loopCount);
                card.tempBackNumber = card.cardData.back_Number + (dataPersistence.gameData.loopCount);
            }
            hasScaled = true;
        }
    }

    private void ResetScaledStats()
    {
        foreach(Card card in turn.enemyDeck.handPile)
        {
            card.tempFrontNumber = card.cardData.front_Number;
            card.tempBackNumber = card.cardData.back_Number;
        }
    }
}
