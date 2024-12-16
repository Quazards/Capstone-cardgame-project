using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 initialScale;
    [HideInInspector] public bool isPointerOverCard = false;
    [HideInInspector] public bool isHovering;
    public bool isNotCardDisplay = true;

    private CardRotation cardRotation;
    private Canvas cardCanvas;
    private GameObject playerHand;
    private GameObject enemyHand;
    private Card currentCard;
    private TurnSystem turn;
    
    private void Start()
    {
        initialScale = Vector3.one;
        cardRotation = GetComponent<CardRotation>();
        currentCard = GetComponent<Card>();
        cardCanvas = GameObject.FindGameObjectWithTag("CardCanvas").GetComponent<Canvas>();
        playerHand = GameObject.FindGameObjectWithTag("Hand");
        enemyHand = GameObject.FindGameObjectWithTag("EnemyArea");
        turn = TurnSystem.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (turn.currentPhase != CombatPhase.CardKeep)
        {
            isPointerOverCard = true;

            if (isNotCardDisplay && !cardRotation.isDragging)
            {   
                isHovering = true;
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (turn.currentPhase != CombatPhase.CardKeep)
        {
            isPointerOverCard = false;

            if (isNotCardDisplay)
            {
                isHovering = false;
                transform.localScale = initialScale;
            }
        }
    }
}
