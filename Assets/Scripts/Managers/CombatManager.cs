using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    [HideInInspector] public PlayAreaManager playArea;
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public EnemyHealth enemyHealth;
    [HideInInspector] public AudioManager audioManager;

    [Header("Health Bar Component")]
    [SerializeField] private HealthBarUI playerHealthBar;
    [SerializeField] private HealthBarUI enemyHealthBar;

    [Header("Shaking Component")]
    public ObjectShake playerComponent;
    public ObjectShake enemyComponent;

    [Header("Damage Popup Component")]
    [SerializeField] private DamagePopup enemyDamagePopup;
    [SerializeField] private DamagePopup playerDamagePopup;

    public int playerShield = 0;
    public int enemyShield = 0;

    public void OnEnable()
    {
        CombatEvents.OnPlayerDamageTaken += OnPlayerDamageTaken;
        CombatEvents.OnEnemyDamageTaken += OnEnemyDamageTaken;
    }

    public void OnDisable()
    {
        CombatEvents.OnPlayerDamageTaken -= OnPlayerDamageTaken;
        CombatEvents.OnEnemyDamageTaken -= OnEnemyDamageTaken;
    }

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
        audioManager = AudioManager.Instance;

        playerHealthBar.SetMaxHealthBar(playerHealth.playerCurrentHealth, playerHealth.playerMaxHealth);
        StartCoroutine(InitializeEnemyHealth());
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
        }
        else if (playerTotalAttack < enemyTotalAttack)
        {
            DealDamageToPlayer(enemyTotalAttack);

        }
    }

    public void DealDamageToPlayer(int damage)
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

        if(damage != 0)
            ActivatePlayerDamageTaken(damage);
        playerDamagePopup.CreatePopup(damage.ToString());

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

    public void DealDamageToEnemy(int damage)
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

        if(damage !=  0)
            ActivateEnemyDamageTaken(damage);
        enemyDamagePopup.CreatePopup(damage.ToString());

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

    public void HealPlayer(int amount)
    {
        playerHealth.PlayerRegenHealth(amount);
        playerHealthBar.SetHealthBar(playerHealth.playerCurrentHealth);
    }

    public void HealEnemy(int amount)
    {
        enemyHealth.EnemyRegenHealth(amount);
        enemyHealthBar.SetHealthBar(enemyHealth.enemyCurrentHealth);

    }
    private IEnumerator InitializeEnemyHealth()
    {
        yield return new WaitForSeconds(0.001f);
        enemyHealthBar.SetMaxHealthBar(enemyHealth.enemyCurrentHealth, enemyHealth.enemyMaxHealth);

    }

    private void OnPlayerDamageTaken(int damage)
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.damageTakenSound);

        playerHealthBar.SetHealthBar(playerHealth.playerCurrentHealth);
        playerComponent.ShakeObject(0.2f, 50f);
        

    }

    private void OnEnemyDamageTaken(int damage)
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.damageTakenSound);

        enemyHealthBar.SetHealthBar(enemyHealth.enemyCurrentHealth);
        enemyComponent.ShakeObject(0.2f, 50f);
        
    }

    private void ActivatePlayerDamageTaken(int damage)
    {
        CombatEvents.InvokeOnPlayerDamageTaken(damage);
    }

    private void ActivateEnemyDamageTaken(int damage)
    {
        CombatEvents.InvokeOnEnemyDamageTaken(damage);
    }

    //for testing
    public void Kill()
    {
        int damage = 1000;
        DealDamageToEnemy(damage);
    }
}
