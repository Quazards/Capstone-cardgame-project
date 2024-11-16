using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth Instance { get; private set; }

    public TextMeshProUGUI enemyHealthText;

    public int enemyMaxHealth = 0;
    public int enemyCurrentHealth;

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


    void Update()
    {
        enemyHealthText.text = enemyCurrentHealth + "/" + enemyMaxHealth;
    }

    public void SetEnemyMaxHealth(int newMaxHealth)
    {
        enemyMaxHealth = newMaxHealth;
        enemyCurrentHealth = enemyMaxHealth;
    }

    public void EnemyTakeDamage(int damage)
    {
        enemyCurrentHealth -= damage;

        if (enemyCurrentHealth < 0)
        {
            enemyCurrentHealth = 0;
        }
    }

    public void EnemyRegenHealth(int amount)
    {
        enemyCurrentHealth += amount;

        if (enemyCurrentHealth > enemyMaxHealth)
        {
            enemyCurrentHealth = enemyMaxHealth;
        }
    }
}
