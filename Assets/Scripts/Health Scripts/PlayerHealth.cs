using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
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

    private void Start()
    {
        SetPlayerMaxHealth(50);
    }

    private void Update()
    {
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

}
