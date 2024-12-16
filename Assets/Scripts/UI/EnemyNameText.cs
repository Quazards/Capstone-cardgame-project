using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyNameText : MonoBehaviour
{
    private TextMeshProUGUI enemyName;
    private EnemyCollectionManager enemyCollection;

    private void Start()
    {
        enemyName = GetComponentInChildren<TextMeshProUGUI>();
        enemyCollection = EnemyCollectionManager.Instance;
    }

    private void Update()
    {
        enemyName.text = enemyCollection.currentEnemy.name;
    }
}
