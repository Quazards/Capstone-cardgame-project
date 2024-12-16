using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayAreaManager : MonoBehaviour
{
    public static PlayAreaManager Instance { get; private set; }

    [SerializeField] private GameObject flipButton;
    [SerializeField] private GameObject playButton;

    public List<Card> cardsInPlayArea = new List<Card>();
    public List<Card> playerCardsInPlay = new List<Card>();
    
    private TurnSystem turn;
    private Deck enemyDeck;

    public bool hasPlayed = false;
    public bool enemyHasEntered = false;

    //Card Holders
    [SerializeField] GameObject playerCardHolder;
    [SerializeField] GameObject enemyCardHolder;

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
        turn = TurnSystem.Instance;
        enemyDeck = TurnSystem.Instance.enemyDeck.GetComponent<Deck>();

        //play button
        playButton.SetActive(false);
        playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play";
        playButton.GetComponent<Button>().onClick.AddListener(PlayAllCards);

        //flip button
        flipButton.SetActive(false);
        flipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Flip";
        flipButton.GetComponent<Button>().onClick.AddListener(FlipAllCards);
    }

    private void Update()
    {
        if(hasPlayed)
        {
            playButton.SetActive(false);
            flipButton.SetActive(true);
        }
        else
        {
            flipButton.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayingCard"))
        {
            Card card = collision.GetComponent<Card>();

            if (card != null)
            {
                if (!card.cardRotation.hasFlipped)
                {

                    if (card.cardData.card_Ownership == CardOwnership.Player && turn.currentEnergy >= 2)
                    {
                        playerCardsInPlay.Add(card);
                    }
                }
            }
            else
            {
                return;
            }


            if (cardsInPlayArea.Count >= 1 && !hasPlayed)
            {
                playButton.SetActive(true);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayingCard"))
        {
            Card card = collision.GetComponent<Card>();
            CardRotation cardRotation = collision.GetComponent<CardRotation>();

            if (card != null && !card.cardRotation.hasFlipped)
            {
                cardRotation.isOverPlayArea = false;

                if (card.cardData.card_Ownership == CardOwnership.Player)
                {
                    card.RefundEnergy(2);
                    playerCardsInPlay.Remove(card);
                }

            }

            CheckCardsInPlayArea();

            if (cardsInPlayArea.Count == 0)
            {
                playButton.SetActive(false);
            }
        }
    }

    public void CheckCardsInPlayArea()
    {
        cardsInPlayArea.Clear();
        Card[] playerCards = playerCardHolder.GetComponentsInChildren<Card>();
        Card[] enemyCards = enemyCardHolder.GetComponentsInChildren<Card>();
        cardsInPlayArea.AddRange(playerCards);
        cardsInPlayArea.AddRange(enemyCards);

        Debug.Log($"Cards in play area: {cardsInPlayArea.Count}");
    }

    private void FlipAllCards()
    {
        if(turn.isMyTurn && turn.currentEnergy >= 1)
        {
            foreach (Card card in cardsInPlayArea)
            {
                card.cardRotation.BeginFlip();
            }
            
            turn.currentEnergy -= 1;

        }
        else
        {
            return;
        }
    }

    public void PlayAllCards()
    {
        CheckCardsInPlayArea();
        if (turn.isMyTurn && !hasPlayed)
        {
            foreach (Card card in cardsInPlayArea)
            {
                if (!card.cardRotation.hasFlipped)
                {
                    card.cardRotation.BeginFlip();
                }
            }
            hasPlayed = true;
        }
    }

    
    public void PlayEnemyCards()
    {
        if(enemyDeck != null)
        {
            foreach(Card enemyCard in enemyDeck.handPile)
            {
                CardRotation cardRotation = enemyCard.GetComponent<CardRotation>();
                cardRotation.isOverPlayArea = true;
                enemyCard.transform.SetParent(enemyCardHolder.transform, false);
            }
        }

        enemyHasEntered = true;
    }

}
