using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRotation : MonoBehaviour
{
    [HideInInspector] public Card currentCard;
    private CardUI currentCardUI;
    private CardMovementAttemp cardMovement;

    private bool proceedCaroutine = true;
    [HideInInspector] public bool hasFlipped = false;

    private void Start()
    {
        currentCard = GetComponent<Card>();
        currentCardUI = GetComponent<CardUI>();
        cardMovement = GetComponent<CardMovementAttemp>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && cardMovement.isPointerOverCard && !cardMovement.isOverPlayArea)
        {
            if (proceedCaroutine)
            {
                StartCoroutine(HandRotate());
            }
        }
    }

    private IEnumerator HandRotate()
    {
        proceedCaroutine = false;


        if (currentCard.cardPosition == CardPosition.Up)
        {
            for (float i = 0; i <= 180f; i += 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    currentCard.cardPosition = CardPosition.Down;

                }
                yield return new WaitForSeconds(0.01f);
            }
            currentCardUI.cardHandle.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            currentCardUI.backNumber.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        else if (currentCard.cardPosition == CardPosition.Down)
        {
            for (float i = 180; i >= 0; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    currentCard.cardPosition = CardPosition.Up;
                }
                yield return new WaitForSeconds(0.01f);
            }
            currentCardUI.cardHandle.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            currentCardUI.frontNumber.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        proceedCaroutine = true;

    }

    private IEnumerator OnPlayFlip()
    {
        float random = Random.Range(0f, 1f);
        proceedCaroutine = false;

        if (random < 0.5f)
        {
            for (float i = 0; i <= 180f; i += 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    currentCard.cardPosition = CardPosition.Down;

                }
                yield return new WaitForSeconds(0.01f);
            }

            currentCardUI.cardHandle.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            currentCardUI.backNumber.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        else
        {
            for (float i = 180; i >= 0; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    currentCard.cardPosition = CardPosition.Up;
                }
                yield return new WaitForSeconds(0.01f);
            }
            currentCardUI.cardHandle.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            currentCardUI.frontNumber.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }


        proceedCaroutine = true;
    }

    public void beginFlip()
    {
        if (cardMovement.isOverPlayArea)
        {

            if (proceedCaroutine)
            {
                StartCoroutine(OnPlayFlip());
                hasFlipped = true;
            }
        }
    }
}
