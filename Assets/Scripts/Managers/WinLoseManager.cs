using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseManager : MonoBehaviour
{
    public static WinLoseManager Instance;

    private EnemyHealth enemyHealth;
    private PlayerHealth playerHealth;
    private TurnSystem turnSystem;

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
        enemyHealth = EnemyHealth.Instance;
        playerHealth = PlayerHealth.Instance;
        turnSystem = TurnSystem.Instance;
    }

    public void CheckWinLoseCondition()
    {
        if (enemyHealth != null && enemyHealth.enemyCurrentHealth <= 0)
        {
            turnSystem.SwitchPhase(CombatPhase.PlayerWin);
            return;
        }
        if (playerHealth != null &&  playerHealth.playerCurrentHealth <= 0)
        {
            turnSystem.SwitchPhase(CombatPhase.PlayerLose);
            return;
        }
    }
}
