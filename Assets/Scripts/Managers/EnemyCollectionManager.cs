using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCollectionManager : MonoBehaviour
{
    public static EnemyCollectionManager Instance;

    [SerializeField] private AllEnemyStatsCollection allEnemyColletion;

    public EnemyHealth enemyHealth;
    public Deck enemyDeck;

    private List<EnemyStats> allEnemies = new List<EnemyStats>();
    public List<EnemyStats> regularEnemies = new List<EnemyStats>();
    public List<EnemyStats> miniBossEnemies = new List<EnemyStats>();
    public List<EnemyStats> bossEnemies = new List<EnemyStats>();

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

    public void AddEnemyStats()
    {
        allEnemies = allEnemyColletion.AllEnemy.ToList();

        foreach (EnemyStats enemy in allEnemies)
        {
            switch (enemy.enemyType)
            {
                case EnemyType.Regular:
                    regularEnemies.Add(enemy);
                    break;
                case EnemyType.Miniboss:
                    miniBossEnemies.Add(enemy);
                    break;
                case EnemyType.Boss:
                    bossEnemies.Add(enemy);
                    break;
            }
        }
    }

    public EnemyStats GetRandomEnemyStat(EnemyType enemyType)
    {
        List<EnemyStats> selectedList = null;

        switch(enemyType)
        {
            case EnemyType.Regular:
                selectedList = regularEnemies;
                break;
            case EnemyType.Miniboss:
                selectedList = miniBossEnemies;
                break;
            case EnemyType.Boss:
                selectedList = bossEnemies;
                break;
        }

        if(selectedList != null && selectedList.Count > 0)
        {
            int randomIndex = Random.Range(0, selectedList.Count);
            return selectedList[randomIndex];
        }
        else 
            return null;
    }

    private void AssignEnemyStat(EnemyType enemyType)
    {
        EnemyStats selectedEnemyStat =  GetRandomEnemyStat(enemyType);

        if(selectedEnemyStat != null)
        {
            enemyHealth.SetEnemyMaxHealth(selectedEnemyStat.maxHealth);

            enemyDeck.currentDeck = selectedEnemyStat.deckList;
            enemyDeck.drawAmount = selectedEnemyStat.enemyDrawAmount;

            enemyDeck.InstantiateDeck();

            Debug.Log($"Assigned {selectedEnemyStat.enemyType} with max health: {selectedEnemyStat.maxHealth}");
        }
        else
            Debug.LogWarning("No enemy stats available for the selected type.");
    }
}
