using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public bool isMyTurn;
    public bool turnStart = false;

    public int maxEnergy = 3;
    public int currentEnergy;
    public int startEnergy;
    public int turnCount = 1;

    public Deck playerDeck;
    public Deck enemyDeck;
    public TextMeshProUGUI playerDeckCountText;
    public TextMeshProUGUI playerDiscardCountText;

    public TextMeshProUGUI energyText;
    public PlayAreaManager playArea;
    public CombatManager combatManager;

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
        combatManager = CombatManager.Instance;


        PlayerMatchStart();
        EnemyMatchStart();

        isMyTurn = true;
        startEnergy = maxEnergy;
        currentEnergy = startEnergy;
        
        if(isMyTurn)
        {
            Debug.Log("This is my turn");
        }
    }

    private void Update()
    {
        if (turnStart)
        {
            playerDeck.TurnStartDraw();
            enemyDeck.TurnStartDraw();
            turnStart = false;
            Debug.Log("turn start");
        }

        //texts
        energyText.text = currentEnergy + "/" + startEnergy;
        playerDeckCountText.text = playerDeck.deckPile.Count.ToString();
        playerDiscardCountText.text = playerDeck.discardPile.Count.ToString();
    }

    public void EndTurn()
    {
        if (!playArea.hasPlayed && playArea.cardsInPlayArea.Count > 0)
        {

            playArea.PlayAllCards();

        }

        isMyTurn = false;
        StartCoroutine(EnemyTurnSimulation(1));
    }

    public void StartTurn()
    {
        isMyTurn = true;
        turnStart = true;
        currentEnergy = startEnergy;
        turnCount += 1;
        playArea.hasPlayed = false;
    }

    private void PlayerMatchStart()
    {

        playerDeck.InstantiateDeck();

        playerDeck.drawAmount += 2;

        playerDeck.TurnStartDraw();

        playerDeck.drawAmount -= 2;
    }

    private void EnemyMatchStart()
    {
        enemyDeck.InstantiateDeck();
        enemyDeck.TurnStartDraw();
    }
    
    private void EndTurnDiscard()
    {
        List<CardMovementAttemp> cardsToDiscard = new List<CardMovementAttemp>();

        foreach(CardMovementAttemp cardMovement in playArea.cardsInPlayArea)
        {
            cardsToDiscard.Add(cardMovement);
        }

        foreach(CardMovementAttemp cardMovement in cardsToDiscard)
        {
            if(cardMovement.card.cardData.card_Ownership != CardOwnership.Enemy)
            {
                playerDeck.Discard(cardMovement.card);
            }
            else
            {
                enemyDeck.Discard(cardMovement.card);
            }
        }
        playArea.cardsInPlayArea.Clear();
        playArea.playerCardsInPlay.Clear();
    }

    private IEnumerator EnemyTurnSimulation(int time)
    {
        yield return new WaitForSeconds(time);
        combatManager.CalculateDamage();
        EndTurnDiscard();
        StartTurn();
    }
}
