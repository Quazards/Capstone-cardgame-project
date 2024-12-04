using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    public static PlayerHealth Instance { get; private set; }
    public TextMeshProUGUI playerHealthText;

    public int playerMaxHealth = 50;
    public int playerCurrentHealth;

    public void Awake()
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

    private void Update()
    {
        if (playerHealthText == null)
            return;

        playerHealthText.text = playerCurrentHealth + "/" + playerMaxHealth;
    }
    public void SetPlayerMaxHealth(int newMaxHealth)
    {
        playerMaxHealth = newMaxHealth;
        playerCurrentHealth = playerMaxHealth;
    }

    public void PlayerTakeDamage(int damage)
    {
        playerCurrentHealth -= damage;

        if (playerCurrentHealth < 0)
        {
            playerCurrentHealth = 0;
        }
    }

    public void PlayerRegenHealth(int amount)
    {
        playerCurrentHealth += amount;

        if (playerCurrentHealth > playerMaxHealth)
        {
            playerCurrentHealth = playerMaxHealth;
        }
    }

    public void LoadData(GameData data)
    {
        this.playerCurrentHealth = data.playerCurrentHealth;
    }

    public void SaveData(ref GameData data)
    {
        data.playerCurrentHealth = this.playerCurrentHealth;
    }
}
