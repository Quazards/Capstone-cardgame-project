using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    [HideInInspector] public PlayAreaManager playArea;
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public EnemyHealth enemyHealth;
    [SerializeField] private HealthBarUI playerHealthBar;
    [SerializeField] private HealthBarUI enemyHealthBar;

    public int playerShield = 0;
    public int enemyShield = 0;

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

    private void Start()
    {
        playArea = PlayAreaManager.Instance;
        playerHealth = PlayerHealth.Instance;
        enemyHealth = EnemyHealth.Instance;

        if (TurnSystem.Instance.currentPhase == CombatPhase.CombatStart)
        {
            enemyHealthBar.SetMaxHealthBar(enemyHealth.enemyCurrentHealth, enemyHealth.enemyMaxHealth);
        }
        playerHealthBar.SetMaxHealthBar(playerHealth.playerCurrentHealth, playerHealth.playerMaxHealth);
    }

    public void CalculateDamage()
    {
        int playerTotalAttack = 0;
        int playerTotalDefense = 0;
        int enemyTotalAttack = 0;
        int enemyTotalDefense = 0;


        foreach(Card card in playArea.cardsInPlayArea)
        {
            int cardValue = 0;

            CardType currentType = card.CurrentCardType();
            

            if(card.cardPosition == CardPosition.Up)
            {
                cardValue = card.tempFrontNumber;
            }
            else
            {
                cardValue = card.tempBackNumber;
            }

            if(currentType == CardType.Attack)
            {
                if(card.cardData.card_Ownership == CardOwnership.Player)
                {
                    playerTotalAttack += cardValue;
                }
                else if(card.cardData.card_Ownership == CardOwnership.Enemy)
                {
                    enemyTotalAttack += cardValue;
                }
            }

            else if(currentType == CardType.Defend)
            {
                if(card.cardData.card_Ownership == CardOwnership.Player)
                {
                    playerTotalDefense += cardValue;
                }
                else if(card.cardData.card_Ownership== CardOwnership.Enemy)
                {
                    enemyTotalDefense += cardValue;
                }
            }
        }
        playerShield = playerTotalDefense;
        enemyShield = enemyTotalDefense;

        if (playerTotalAttack > enemyTotalAttack)
        {
            DealDamageToEnemy(playerTotalAttack);
            enemyHealthBar.SetHealthBar(enemyHealth.enemyCurrentHealth);
        }
        else if (playerTotalAttack < enemyTotalAttack)
        {
            DealDamageToPlayer(enemyTotalAttack);
            playerHealthBar.SetHealthBar(playerHealth.playerCurrentHealth);

        }
    }

    private void DealDamageToPlayer(int damage)
    {
        if(playerShield > 0)
        {
            if(damage <= playerShield)
            {
                playerShield -= damage;
                damage = 0;
            }
            else
            {
                damage -= playerShield;
                playerShield = 0;
            }
        }
        playerHealth.PlayerTakeDamage(damage);

        foreach (var card in playArea.cardsInPlayArea)
        {
            if (card.isBuffed)
            {
                card.RevertBuff();
            }
        }

        if(playerHealth.playerCurrentHealth <= 0)
        {
            TurnSystem.Instance.SwitchPhase(CombatPhase.PlayerLose);
        }
    }

    private void DealDamageToEnemy(int damage)
    {
        if(enemyShield > 0)
        {
            if(damage <= enemyShield)
            {
                enemyShield -= damage;
                damage = 0;
            }
            else
            {
                damage -= enemyShield;
                enemyShield = 0;
            }
        }
        enemyHealth.EnemyTakeDamage(damage);

        foreach (var card in playArea.cardsInPlayArea)
        {
            if (card.isBuffed)
            {
                card.RevertBuff();
            }
        }

        if (enemyHealth.enemyCurrentHealth <= 0)
        {
            TurnSystem.Instance.SwitchPhase(CombatPhase.PlayerWin);
        }
    }


    //for testing
    public void Kill()
    {
        int damage = 1000;
        DealDamageToEnemy(damage);
    }
}
