using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager Instance;

    public int encounterCount = 0;
    private EnemyCollectionManager enemyCollectionManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
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
        enemyCollectionManager = EnemyCollectionManager.Instance;

        enemyCollectionManager.AddEnemyStats();
        StartCombatEncounter();
    
    }

    private EnemyType GetEnemyType(int encounterCount)
    {
        if (encounterCount % 5 == 0)
        {
            return EnemyType.Boss;
        }
        else if (encounterCount % 3 == 0)
        {
            return EnemyType.Miniboss;
        }
        else
        {
            return EnemyType.Regular;
        }

    }

    public void StartCombatEncounter()
    {
        encounterCount++;

        EnemyType enemyType = GetEnemyType(encounterCount);

        enemyCollectionManager.AssignEnemyStat(enemyType);

        enemyCollectionManager.enemyDeck.InstantiateDeck();

        enemyCollectionManager.enemyDeck.TurnStartDraw();

        PlayerDeckManager.Instance.PlayerStartEncounter();
    }

}
