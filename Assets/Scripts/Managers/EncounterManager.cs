using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour, IDataPersistence
{
    public static EncounterManager Instance;
    public int encounterCount = 0;
    public int loopCount = 0;
    public bool isFirstEncounter = true;

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
        if (enemyCollectionManager == null) return;
        enemyCollectionManager.AddEnemyStats();
        StartCombatEncounter();
    
    }

    private EnemyType GetEnemyType(int encounterCount)
    {
        if (encounterCount == 5)
        {
            return EnemyType.Boss;
        }
        else if (encounterCount == 3)
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
        if (!isFirstEncounter)
        {
            encounterCount++;

            if (encounterCount > 5)
            {
                encounterCount = 0;
                loopCount++;
            }
        }

        EnemyType enemyType = GetEnemyType(encounterCount);

        enemyCollectionManager.AssignEnemyStat(enemyType);
        enemyCollectionManager.enemyDeck.InstantiateDeck();
        enemyCollectionManager.enemyDeck.TurnStartDraw();

        PlayerDeckManager.Instance.PlayerStartEncounter();
        TurnSystem.Instance.SwitchPhase(CombatPhase.CombatStart);
        isFirstEncounter = false;

        Debug.Log($"Encounter count: {DataPersistenceManager.instance.gameData.encounterCount}/{encounterCount}, " +
            $"loop Count: {DataPersistenceManager.instance.gameData.loopCount}/{loopCount}");
    }

    public void ResetEncounterState()
    {
        this.encounterCount = 0;
        this.loopCount = 0;
        isFirstEncounter = true;
    }

    public void LoadData(GameData data)
    {
        this.encounterCount = data.encounterCount;
        this.loopCount = data.loopCount;
        this.isFirstEncounter = data.isFirstEncounter;
    }

    public void SaveData(ref GameData data)
    {
        data.encounterCount = this.encounterCount;
        data.loopCount = this.loopCount;
        data.isFirstEncounter = this.isFirstEncounter;
    }
}
