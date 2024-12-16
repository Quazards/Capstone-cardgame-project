using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public CardPile currentDeck;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Canvas cardCanvas;
    private TurnSystem turnSystem;
    private AudioManager audioManager;

    public int drawAmount = 1;

    public List<Card> deckPile = new();
    public List<Card> discardPile = new();
    public List<Card> handPile { get; private set; } = new();

    private void Start()
    {
        turnSystem = TurnSystem.Instance;
        audioManager = AudioManager.Instance;
    }

    public void ClearAll()
    {
        foreach (var card in deckPile)
        {
            Destroy(card.gameObject);
        }
        foreach (var card in discardPile)
        {
            Destroy(card.gameObject);
        }
        foreach (var card in handPile)
        {
            Destroy(card.gameObject);
        }

        deckPile.Clear();
        discardPile.Clear();
        handPile.Clear();
    }

    public void InstantiateDeck()
    {
        if (currentDeck == null) return;

        for(int i = 0; i < currentDeck.cardsInPile.Count; i++)
        {
            Card card = Instantiate(cardPrefab, cardCanvas.transform);
            card.SetUp(currentDeck.cardsInPile[i]);
            card.gameObject.SetActive(false);
            deckPile.Add(card);
        }
        Shuffle();
    }

    public void Shuffle()
    {
        for(int i = deckPile.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = deckPile[i];
            deckPile[i] = deckPile[j];
            deckPile[j] = temp;
        }
    }

    public void TurnStartDraw()
    {
        StartCoroutine(DrawCardsWithAnimation(drawAmount));
    }

    public IEnumerator DiscardHard()
    {
        List<Card> cardsToDiscard = new List<Card>();

        foreach(Card card in handPile)
        {
            if (!card.keepCard)
            {
                cardsToDiscard.Add(card);
            }
        }

        foreach(Card card in cardsToDiscard)
        {
            yield return StartCoroutine(Discard(card));
        }
    }

    public IEnumerator Discard(Card card)
    {
        if (handPile.Contains(card))
        {
            card.ResetStoredEnergy();
            handPile.Remove(card);
            discardPile.Add(card);

            Vector3 startPosition = card.transform.position;
            Vector3 endPosition = card.transform.position;

            if (card.cardData.card_Ownership == CardOwnership.Player)
            {
                endPosition = GameObject.FindGameObjectWithTag("PlayerDiscardPile").transform.position;
            }
            else if (card.cardData.card_Ownership == CardOwnership.Enemy)
            {
                endPosition = GameObject.FindGameObjectWithTag("EnemyDiscardPile").transform.position;
            }

            yield return StartCoroutine(CardDiscardAnimation(card, startPosition, endPosition));

            card.gameObject.SetActive(false);
        }
    }

    private IEnumerator DrawCardsWithAnimation(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (deckPile.Count == 0 && discardPile.Count != 0)
            {
                deckPile.AddRange(discardPile);
                discardPile.Clear();
                Shuffle();
            }

            if (deckPile.Count > 0)
            {
                Card drawnCard = deckPile[0];
                handPile.Add(drawnCard);
                deckPile.RemoveAt(0);

                drawnCard.gameObject.SetActive(true);
                drawnCard.keepCard = false;
                drawnCard.ResetStoredEnergy();

                var cardRotation = drawnCard.GetComponent<CardRotation>();
                if (cardRotation != null)
                {
                    cardRotation.hasFlipped = false;
                    cardRotation.isOverPlayArea = false;
                }

                Vector3 startPosition = drawnCard.transform.position;
                Vector3 endPosition = drawnCard.transform.position;
                if (drawnCard.cardData.card_Ownership == CardOwnership.Player)
                {
                    startPosition = GameObject.FindGameObjectWithTag("PlayerDeckPile").transform.position;
                    endPosition = GameObject.FindGameObjectWithTag("Hand").transform.position;
                }
                else if (drawnCard.cardData.card_Ownership == CardOwnership.Enemy)
                {
                    startPosition = GameObject.FindGameObjectWithTag("EnemyDeckPile").transform.position;
                    endPosition = GameObject.FindGameObjectWithTag("EnemyArea").transform.position;
                }

                yield return StartCoroutine(CardDrawAnimation(drawnCard, startPosition, endPosition));


                if (drawnCard.cardData.card_Ownership == CardOwnership.Player)
                {
                    GameObject hand = GameObject.FindGameObjectWithTag("Hand");
                    drawnCard.transform.SetParent(hand.transform, true);
                    var handLayout = hand.GetComponent<HorizontalLayoutGroup>();
                    handLayout.childControlHeight = true;
                    handLayout.childControlHeight = false;
                }
                else if (drawnCard.cardData.card_Ownership == CardOwnership.Enemy)
                {
                    drawnCard.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyArea").transform, true);
                }

                if (audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.cardDrawSound);
                }
            }
        }
    }


    private IEnumerator CardDrawAnimation(Card card, Vector3 startPosition, Vector3 endPosition)
    {
        card.transform.position = startPosition;
        card.transform.localScale = Vector3.zero;

        float animationTime = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < animationTime)
        {
            float time = elapsedTime / animationTime;

            card.transform.position = Vector3.Lerp(startPosition, endPosition, time);
            card.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        card.transform.position = endPosition;
        card.transform.localScale = Vector3.one;
    }

    private IEnumerator CardDiscardAnimation(Card card, Vector3 startPosition, Vector3 endPosition)
    {
        card.transform.position = startPosition;
        card.transform.localScale = Vector3.zero;

        float animationTime = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < animationTime)
        {
            float time = elapsedTime / animationTime;

            card.transform.position = Vector3.Lerp(startPosition, endPosition, time);
            card.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        card.transform.position = endPosition;
        card.transform.localScale = Vector3.zero;
    }

    public void AdditionalDraw()
    {
        if(TurnSystem.Instance.currentEnergy >= 1 && CheckCardAvailability())
        {
            TurnStartDraw();
            TurnSystem.Instance.currentEnergy -= 1;
        }
        else
        {
            return;
        }
    }

    public void AnyAmountDraw(int amount)
    {
        drawAmount += amount;
        TurnStartDraw();
        drawAmount -= amount;
    }

    private bool CheckCardAvailability()
    {
        if(deckPile.Count == 0 && discardPile.Count == 0)
            return false;
        else
            return true;
    }

}