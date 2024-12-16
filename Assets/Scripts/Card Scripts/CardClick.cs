using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool isCursorOverCard = false;
    public bool isCardSelected = false;
    private TurnSystem turn;
    private Card currentCard;
    private GameObject hand;
    private GameObject keepArea;

    private void Start()
    {
        currentCard = GetComponent<Card>();
        turn = TurnSystem.Instance;
        keepArea = GameObject.FindGameObjectWithTag("KeepArea");
        hand = GameObject.FindGameObjectWithTag("Hand");

    }

    private void Update()
    {
        if(turn.currentPhase == CombatPhase.CardKeep)
        {
            if(Input.GetMouseButtonDown(0) && isCursorOverCard)
            {
                if(!isCardSelected) 
                    SelectCard();
                else
                    DeselectCard();
            }
        }

        if(turn.currentPhase == CombatPhase.TurnStart)
        {
            currentCard.keepCard = false;
        }
    }

    private void SelectCard()
    {
        if (turn.cardsToKeep.Count < 4)
        {
            isCardSelected = true;
            currentCard.transform.SetParent(keepArea.transform, false);
            currentCard.keepCard = true;
            turn.cardsToKeep.Add(currentCard);
        }
    }

    public void DeselectCard()
    {
        isCardSelected = false;
        currentCard.transform.SetParent(hand.transform, false);
        currentCard.keepCard = false;
        turn.cardsToKeep.Remove(currentCard);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isCursorOverCard = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isCursorOverCard = false;
    }
}
