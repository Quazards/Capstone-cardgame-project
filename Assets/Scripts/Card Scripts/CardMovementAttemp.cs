using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovementAttemp : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform newParent;
    [HideInInspector] public bool isPlayerCard = false;
    [HideInInspector] public bool isDragging = false;

    private Canvas cardCanvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
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
        canvasGroup = GetComponent<CanvasGroup>();
        card = GetComponent<Card>();
        cardUI = GetComponent<CardUI>();
        cardRotation = GetComponent<CardRotation>();
        cardHover = GetComponent<CardHover>();
        turnSystem = TurnSystem.Instance;
        audioManager = AudioManager.Instance;

        Hand = GameObject.FindGameObjectWithTag("Hand");

    }

    private void Update()
    {
        if(cardRotation.isDragging) 
            canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (TurnSystem.Instance.isMyTurn && !cardRotation.hasFlipped && isPlayerCard 
            && TurnSystem.Instance.currentPhase != CombatPhase.CardKeep)
        {
            rectTransform.anchoredPosition += (eventData.delta / cardCanvas.scaleFactor);
            transform.SetParent(cardCanvas.transform, true);
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (turnSystem.currentPhase == CombatPhase.CardKeep)
            return;

        newParent = Hand.transform;
        canvasGroup.blocksRaycasts = false;
        cardRotation.isDragging = true;


        if (card.cardData.card_Ownership == CardOwnership.Player)
        {
            isPlayerCard = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (turnSystem.currentPhase == CombatPhase.CardKeep)
            return;

        hoveredObject = eventData.pointerEnter;
        canvasGroup.blocksRaycasts = true;
        cardRotation.isDragging = false;

        if (hoveredObject.CompareTag("PlayerCardHolder") && hoveredObject != null 
            && card.cardData.card_Ownership == CardOwnership.Player && !cardHover.isHovering)
        {
            transform.SetParent(newParent, false);
            isPlayerCard = false;

            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.cardPutSound);
            }
            
        }
        else if (card.cardData.card_Ownership == CardOwnership.Player)
        {
            transform.SetParent(newParent, false);
            isPlayerCard = false;
        }

    }

}
