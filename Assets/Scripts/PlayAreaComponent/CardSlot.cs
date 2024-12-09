using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    private TurnSystem turnSystem;
    private PlayAreaManager playArea;

    private void Start()
    {
        turnSystem = TurnSystem.Instance;
        playArea = PlayAreaManager.Instance;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        Card droppedCard = droppedItem.GetComponent<Card>();
        CardMovementAttemp cardMovement = droppedItem.GetComponent<CardMovementAttemp>();
        CardRotation cardRotation = droppedItem.GetComponent<CardRotation>();

        playArea.PlayEnemyCards();

        if (playArea.playerCardsInPlay.Count <= 1 && !playArea.hasPlayed)
        {
            cardMovement.newParent = transform;
            cardRotation.isOverPlayArea = true;
            playArea.cardsInPlayArea.Add(droppedCard);
        }
        else if (playArea.playerCardsInPlay.Count > 1 && turnSystem.currentEnergy >= 2 && !playArea.hasPlayed)
        {
            cardMovement.newParent = transform;
            droppedCard.ConsumeEnergy(2);
            cardRotation.isOverPlayArea = true;
            playArea.cardsInPlayArea.Add(droppedCard);
        }
        else if (playArea.playerCardsInPlay.Count > 1 && turnSystem.currentEnergy < 2 || playArea.hasPlayed)
        {
            return;
        }

    }


}
