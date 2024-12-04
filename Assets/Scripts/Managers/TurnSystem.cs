using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public CombatPhase currentPhase;

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

    private List<(CardEffect effect, Card currentCard, List<Card> cardList, int turnAmount)> scheduledEffects =
        new List<(CardEffect effect, Card currentCard, List<Card> cardList, int turnAmount)>();

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
        
    }

    private void Update()
    {
        //texts
        energyText.text = currentEnergy + "/" + startEnergy;
        playerDeckCountText.text = playerDeck.deckPile.Count.ToString();
        playerDiscardCountText.text = playerDeck.discardPile.Count.ToString();
    }

    public void SwitchPhase(CombatPhase newPhase)
    {
        currentPhase = newPhase;

        switch(currentPhase)
        {
            case CombatPhase.TurnStart:
                TurnStartPhase();
                break;
            case CombatPhase.TurnEnd:
                TurnEndPhase();
                break;
            case CombatPhase.PlayerWin:
                PlayerWinPhase();
                break;
            case CombatPhase.PlayerLose:
                PlayerLosePhase();
                break;
            case CombatPhase.CombatStart:
                CombatStartPhase();
                break;
        }
    }

    public void CombatStartPhase()
    {
        isMyTurn = true;
        startEnergy = maxEnergy;
        currentEnergy = startEnergy;
    }

    public void TurnEndPhase()
    {
        Debug.Log($"Phase switched to {currentPhase}");

        if(!playArea.enemyHasEntered)
        {
            playArea.PlayEnemyCards();
        }

        if (!playArea.hasPlayed && playArea.cardsInPlayArea.Count > 0)
        {
            playArea.PlayAllCards();
        }

        isMyTurn = false;
        StartCoroutine(SimulateEndTurn(0.5f));
    }

    public void TurnStartPhase()
    {
        Debug.Log($"Phase switched to {currentPhase}");

        isMyTurn = true;
        turnStart = true;

        currentEnergy = startEnergy;
        turnCount += 1;
        playArea.hasPlayed = false;
        playArea.enemyHasEntered = false;

        playerDeck.TurnStartDraw();
        enemyDeck.TurnStartDraw();

        ExecuteScheduledEffect();
    }

    public void PlayerWinPhase()
    {
        Debug.Log($"Phase switched to {currentPhase}");

        PlayerDeckManager.Instance.PlayerEndEncounter();
        RewardManager.Instance.ShowRewardScreen();
    }

    public void PlayerLosePhase()
    {
        SceneController.Instance.LoadSceneByName("MainMenu");
        Debug.Log($"Phase switched to {currentPhase}");
    }

    public void EndTurnButton()
    {
        SwitchPhase(CombatPhase.TurnEnd);
    }

    
    private void EndTurnDiscard()
    {
        List<Card> cardsToDiscard = new List<Card>();

        foreach(Card card in playArea.cardsInPlayArea)
        {
            cardsToDiscard.Add(card);
        }

        foreach(Card card in cardsToDiscard)
        {
            if(card.cardData.card_Ownership != CardOwnership.Enemy)
            {
                playerDeck.Discard(card);
            }
            else
            {
                enemyDeck.Discard(card);
            }
        }
        playArea.cardsInPlayArea.Clear();
        playArea.playerCardsInPlay.Clear();
    }

    public void RegisterEffect(CardEffect effect, Card currentCard, List<Card> cardList, int turnAmount)
    {
        scheduledEffects.Add((effect, currentCard, cardList, turnAmount));
    }

    public void ExecuteScheduledEffect()
    {
        if (scheduledEffects.Count > 0)
        {
            for (int i = scheduledEffects.Count - 1; i >= 0; i--)
            {
                var (effect, currentCard, cardList, turnAmount) = scheduledEffects[i];
                if (turnAmount == 0)
                {
                    scheduledEffects.RemoveAt(i);

                }
                else
                {
                    effect.ExecuteEffect(currentCard, cardList);
                    scheduledEffects[i] = (effect, currentCard, cardList, turnAmount - 1);
                }
            }
        }
    }

    private void ActivateAllCards()
    { 
        foreach(var card in playArea.cardsInPlayArea)
        {
            card.PlayCard(playArea.cardsInPlayArea);
        }
    }

    private IEnumerator SimulateEndTurn(float time)
    {
        ActivateAllCards();
        yield return new WaitForSeconds(time * 2);
        combatManager.CalculateDamage();

        EndTurnDiscard();
        SwitchPhase(CombatPhase.TurnStart);
    }
}
