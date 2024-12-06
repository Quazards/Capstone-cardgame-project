using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovementAttemp : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform newParent;
    [HideInInspector] public bool isPlayerCard = false;

    private Canvas cardCanvas;
    private RectTransform rectTransform;
    public Card card;
    private GameObject Hand;
    private CardUI cardUI;
    private CardHover cardHover;
    public CardRotation cardRotation;

    public GameObject hoveredObject;
    public TurnSystem turnSystem;
    private AudioManager audioManager;


    public Image cardImageComponent;
    public Image cardBackgroundComponent;
    public Image cardBorderComponent;
    public Image cardHandleComponent;

    private readonly string CANVAS_TAG = "CardCanvas";

    private void Start()
    {
        cardCanvas = GameObject.FindGameObjectWithTag(CANVAS_TAG).GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        card = GetComponent<Card>();
        cardUI = GetComponent<CardUI>();
        cardRotation = GetComponent<CardRotation>();
        cardHover = GetComponent<CardHover>();
        turnSystem = TurnSystem.Instance;
        audioManager = AudioManager.Instance;

        Hand = GameObject.FindGameObjectWithTag("Hand");

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (TurnSystem.Instance.isMyTurn && !cardRotation.hasFlipped && isPlayerCard)
        {
            rectTransform.anchoredPosition += (eventData.delta / cardCanvas.scaleFactor);
            transform.SetParent(cardCanvas.transform, true);
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        newParent = Hand.transform.parent;
        cardImageComponent.raycastTarget = false;
        cardBackgroundComponent.raycastTarget = false;
        cardBorderComponent.raycastTarget = false;
        cardHandleComponent.raycastTarget = false;
        cardRotation.isDragging = true;


        if (card.cardData.card_Ownership == CardOwnership.Player)
        {
            isPlayerCard = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        hoveredObject = eventData.pointerEnter;
        cardImageComponent.raycastTarget = true;
        cardBackgroundComponent.raycastTarget = true;
        cardBorderComponent.raycastTarget = true;
        cardHandleComponent.raycastTarget = true;
        cardRotation.isDragging = false;

        if (hoveredObject.CompareTag("PlayerCardHolder") && hoveredObject != null 
            && card.cardData.card_Ownership == CardOwnership.Player && !cardHover.isHovering)
        {
            transform.SetParent(newParent, false);

            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.cardPutSound);
            }
            
        }
        else if (isPlayerCard && hoveredObject != null)
        {
            transform.SetParent(Hand.transform, false);
            
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayArea"))
        {
            cardRotation.isOverPlayArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayArea"))
        {
            cardRotation.isOverPlayArea = false;
        }
    }

}
