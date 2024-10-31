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
    public List<CardMovementAttemp> cardsInPlayArea = new List<CardMovementAttemp>();
    public List<CardMovementAttemp> playerCardsInPlay = new List<CardMovementAttemp>();
    
    private TurnSystem turn;
    private Deck enemyDeck;

    public bool hasPlayed = false;

    //Card Holders
    [SerializeField] Transform playerCardHolder;
    [SerializeField] Transform enemyCardHolder;

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
            CardMovementAttemp card = collision.GetComponent<CardMovementAttemp>();
            if (card.card != null)
            {
                if (!card.hasFlipped)
                {
                    cardsInPlayArea.Add(card);
                    Debug.Log("Card is added to play area: " + card.card.cardData.card_Name);

                    if (card.card.cardData.card_Ownership == CardOwnership.Player)
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
            CardMovementAttemp card = collision.GetComponent<CardMovementAttemp>();

            if (card != null && !card.hasFlipped)
            {
                
                if (playerCardsInPlay.Count > 1 && card.card.hasUsedPlayEnergy == true)
                {
                    turn.currentEnergy += 2;
                    card.card.hasUsedPlayEnergy = false;

                    Debug.Log("Card is removed from play area: " + card.card.cardData.card_Name);
                }

                cardsInPlayArea.Remove(card);
                playerCardsInPlay.Remove(card);

            }

            if (cardsInPlayArea.Count == 0)
            {
                playButton.SetActive(false);
                //flipButton.SetActive(false);
            }
        }
    }

    private void FlipAllCards()
    {
        if(turn.isMyTurn && turn.currentEnergy >= 1)
        {
            foreach (CardMovementAttemp card in cardsInPlayArea)
            {
                card.beginFlip();
            }
            
            turn.currentEnergy -= 1;

        }
        else
        {
            Debug.Log("You don't have enough energy to flip");
        }
    }

    public void PlayAllCards()
    {
        if (turn.isMyTurn && !hasPlayed)
        {
            foreach (CardMovementAttemp card in cardsInPlayArea)
            {
                if (!card.hasFlipped)
                {
                    card.beginFlip();
                }
            }
            hasPlayed = true;
            Debug.Log("Card(s) has been played");
        }
    }

    public void PlayEnemyCards()
    {

        if(enemyDeck != null)
        {
            foreach(Card enemyCard in enemyDeck.handPile)
            {
                CardMovementAttemp enemyCardMovement = enemyCard.GetComponent<CardMovementAttemp>();
                if(enemyCardMovement != null)
                {
                    enemyCard.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyCardHolder").transform, false);

                }
            }
        }
    }




}
