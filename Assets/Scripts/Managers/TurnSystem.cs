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
    public bool isTurnEndButtonPressed = false;

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
        if (!playArea.enemyHasEntered)
        {
            playArea.PlayEnemyCards();
        }

        if (playArea.hasPlayed)
        {
            Debug.Log("Play cards routine has activated before playing all cards");
        }

        playArea.PlayAllCards();

        if (playArea.hasPlayed)
        {
            Debug.Log("Play cards routine has activated after playing all cards");
        }

        Debug.Log($"Play cards routine has activated, with {playArea.hasPlayed} and {playArea.cardsInPlayArea.Count}");

        isMyTurn = false;
        StartCoroutine(SimulateEndTurn(1f));
    }

    public void TurnStartPhase()
    {
        isMyTurn = true;
        turnStart = true;
        isTurnEndButtonPressed = false;

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
        PlayerDeckManager.Instance.PlayerEndEncounter();
        RewardManager.Instance.ShowRewardScreen();
    }

    public void PlayerLosePhase()
    {
        SceneController.Instance.LoadSceneByName("MainMenu");
    }

    public void EndTurnButton()
    {
        if (!isTurnEndButtonPressed)
        {
            SwitchPhase(CombatPhase.TurnEnd);
            isTurnEndButtonPressed = true;
        }
    }
    
    private IEnumerator HandleAutoplay()
    {
        if (!playArea.enemyHasEntered)
        {
            playArea.PlayEnemyCards();
        }

        if (!playArea.hasPlayed)
        {
            playArea.PlayAllCards();
            Debug.Log("Play cards routine has activated");
        }
        yield return new WaitForSeconds(0.01f);
    }

    private IEnumerator EndTurnDiscard()
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
                yield return StartCoroutine(playerDeck.Discard(card));
            }
            else
            {
                yield return StartCoroutine(enemyDeck.Discard(card));
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
        yield return new WaitForSeconds(time);
        combatManager.CalculateDamage();
        StartCoroutine(EndTurnDiscard());
        yield return new WaitForSeconds(time);
        SwitchPhase(CombatPhase.TurnStart);
    }
}
