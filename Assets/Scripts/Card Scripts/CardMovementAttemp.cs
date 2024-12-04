using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovementAttemp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform newParent;

    [HideInInspector] public bool isOverPlayArea = false;
    [HideInInspector] public bool isPlayerCard = false;
    [HideInInspector] public bool isPointerOverCard = false;

    private Canvas cardCanvas;
    private RectTransform rectTransform;
    public Card card;
    private GameObject Hand;
    private CardUI cardUI;
    private CardRotation cardRotation;

    public GameObject hoveredObject;
    public TurnSystem turnSystem;

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
        turnSystem = TurnSystem.Instance;
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

        if (hoveredObject.CompareTag("PlayerCardHolder") && hoveredObject != null && card.cardData.card_Ownership == CardOwnership.Player)
        {
            transform.SetParent(newParent, false);
            
        }
        else if (isPlayerCard && hoveredObject != null)
        {
            transform.SetParent(Hand.transform, false);
            
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOverCard = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOverCard = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayArea"))
        {
            isOverPlayArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayArea"))
        {
            isOverPlayArea = false;
        }
    }

}
