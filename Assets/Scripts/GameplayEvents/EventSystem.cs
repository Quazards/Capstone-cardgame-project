using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    PlayerHealth playerHealth;
    private int playerHalfHealth;
    private int randomID;
    private bool isOnCooldown = false;
    public float cooldownTime = 2f;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Start()
    {
	    playerHealth = PlayerHealth.Instance;
        playerHalfHealth = playerHealth.playerMaxHealth / 2;
    }

    [HideInInspector] public void grabTheMug()
    {
        playerHealth.playerCurrentHealth = playerHealth.playerCurrentHealth + playerHalfHealth;
        if (playerHealth.playerCurrentHealth > playerHealth.playerMaxHealth)
        {
            playerHealth.playerCurrentHealth = playerHealth.playerMaxHealth;
        }
        Debug.Log("Player is healed half health");
        StartCoroutine(CooldownRoutine());
        if (!isOnCooldown)
        {
            SceneController.Instance.LoadSceneByName("CombatScene");
        }
    }

    [HideInInspector] public void lonelyBag()
    {
        randomID = Random.Range(1, 3);
        switch (randomID)
        {
            case 1:
            Debug.Log("10 health Restored.");
            playerHealth.PlayerRegenHealth(10);
            break;
            case 2:
            Debug.Log("pick 2 cards out of 4");
            break;
            case 3:
            Debug.Log("-10 health!");
            playerHealth.PlayerTakeDamage(10);
            break;
        }
    }

    [HideInInspector] public void changeScene()
    {
        SceneController.Instance.LoadSceneByName("CombatScene");
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
}
