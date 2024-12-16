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
    private bool isTurnEndButtonPressed = false;
    private bool isCardKeepButtonPressed = false;
    private EnemyScalingManager enemyScaling;

    public int maxEnergy = 3;
    public int currentEnergy;
    public int startEnergy;
    public int turnCount = 1;

    public Deck playerDeck;
    public Deck enemyDeck;
    public TextMeshProUGUI energyText;

    public PlayAreaManager playArea;
    public CombatManager combatManager;
    public KeepAreaManager keepArea;
    public GameObject keepPanel;
    public Canvas winCanvas;

    public List<Card> cardsToKeep = new List<Card>();
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
        enemyScaling = EnemyScalingManager.Instance;
    }

    private void Start()
    {
        combatManager = CombatManager.Instance;
    }

    private void Update()
    {
        //texts
        energyText.text = currentEnergy + "/" + startEnergy;
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
            case CombatPhase.CardKeep:
                ResetKeptCards();
                break;
        }
    }

    public void CombatStartPhase()
    {
        isMyTurn = true;
        startEnergy = maxEnergy;
        currentEnergy = startEnergy;
        enemyScaling.hasScaled = false;
    }

    public void TurnEndPhase()
    {
        if (!playArea.enemyHasEntered)
        {
            playArea.PlayEnemyCards();
        }
        playArea.PlayAllCards();
        isMyTurn = false;
        StartCoroutine(SimulateEndTurn(1f));
    }

    public void TurnStartPhase()
    {
        isMyTurn = true;
        turnStart = true;
        isTurnEndButtonPressed = false;
        isCardKeepButtonPressed = false;

        currentEnergy = startEnergy;
        turnCount += 1;
        playArea.hasPlayed = false;
        playArea.enemyHasEntered = false;

        playerDeck.TurnStartDraw();
        enemyDeck.TurnStartDraw();

        ExecuteScheduledEffect();
        foreach (Card card in playerDeck.handPile)
        {
            card.ResetStoredEnergy();
        }
    }

    public void PlayerWinPhase()
    {
        PlayerDeckManager.Instance.PlayerEndEncounter();
        if (EncounterManager.Instance.encounterCount == 5)
        {
            winCanvas.gameObject.SetActive(true);
        }
        else
        {
            RewardManager.Instance.ShowRewardScreen();
        }
        Debug.Log($"encounter count: {EncounterManager.Instance.encounterCount}");
    }

    public void PlayerLosePhase()
    {
        SceneController.Instance.LoadSceneByName("MainMenu");
    }

    public void EndTurnButton()
    {
        keepArea.ReturnCardsToHand();

        if (!isTurnEndButtonPressed)
        {
            SwitchPhase(CombatPhase.TurnEnd);
            isTurnEndButtonPressed = true;
        }
    }

    public void CardKeepButton()
    {
        if (!isCardKeepButtonPressed)
        {

            if (playerDeck.handPile.Count <= 4)
            {
                foreach (Card card in playerDeck.handPile)
                {
                    card.keepCard = true;
                }
                SwitchPhase(CombatPhase.TurnEnd);
            }
            else
            {
                SwitchPhase(CombatPhase.CardKeep);
                GameObject.FindGameObjectWithTag("CardCanvas").SetActive(false);
                keepPanel.SetActive(true);
            }
            isCardKeepButtonPressed = true;
        }
    }

    public void ResetKeptCards()
    {
        cardsToKeep.Clear();
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
        StartCoroutine(playerDeck.DiscardHard());
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
