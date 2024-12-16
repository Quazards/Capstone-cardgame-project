using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyRankText : MonoBehaviour
{
    private TextMeshProUGUI enemyRank;
    private EnemyCollectionManager enemyCollection;

    private void Start()
    {
        enemyRank = GetComponentInChildren<TextMeshProUGUI>();
        enemyCollection = EnemyCollectionManager.Instance;
    }

    private void Update()
    {
        SetNameByType(enemyCollection.currentEnemy.enemyType);
    }

    private void SetNameByType(EnemyType enemyType)
    {
        switch(enemyType)
        {
            case EnemyType.Regular:
                enemyRank.text = "Reg";
                break;
            case EnemyType.Miniboss:
                enemyRank.text = "Mini-B";
                break;
            case EnemyType.Boss:
                enemyRank.text = "Boss";
                break;
        }
    }
}
