using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRotation : MonoBehaviour
{
    [HideInInspector] public Card currentCard;
    private CardUI currentCardUI;
    private CardHover cardHover;
    private AudioManager audioManager;

    private bool proceedCaroutine = true;
    [HideInInspector] public bool hasFlipped = false;
    [HideInInspector] public bool isOverPlayArea = false;
    [HideInInspector] public bool isDragging = false;

    private void Start()
    {
        currentCard = GetComponent<Card>();
        currentCardUI = GetComponent<CardUI>();
        cardHover = GetComponent<CardHover>();
        audioManager = AudioManager.Instance;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && cardHover.isPointerOverCard && !isOverPlayArea)
        {
            if (proceedCaroutine)
            {
                StartCoroutine(HandRotate());
            }
        }
    }

    private IEnumerator HandRotate()
    {
        CardPosition currentPosition = currentCard.cardPosition;
        proceedCaroutine = false;
        bool hasPlayedSFX = false;

        float startRotation = (currentPosition == CardPosition.Up) ? 0f : 180f;
        float targetRotation = (currentPosition == CardPosition.Up) ? 180f : 0f;

        for (float i = startRotation; Mathf.Abs(i - targetRotation) > 0.01f; 
            i += (currentPosition == CardPosition.Up) ? 10f : -10f)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);

            if(!hasPlayedSFX && Mathf.Abs(i - 90) < 10f)
            {
                hasPlayedSFX = true;
                if(audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.cardFlipSound);
                }
            }

            if(i == 90)
            {
                if (currentPosition == CardPosition.Down)
                {
                    currentCardUI.frontCardName.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    currentCardUI.frontCardDescription.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                }
                else if (currentPosition == CardPosition.Up)
                {
                    currentCardUI.backCardName.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                    currentCardUI.frontCardDescription.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                }
                currentCard.cardPosition = (currentPosition == CardPosition.Up) ? CardPosition.Down : CardPosition.Up;
            }

            yield return new WaitForSeconds(0.01f);
        }
        ResetUIElements(currentPosition);
        proceedCaroutine = true;

    }

    private IEnumerator OnPlayFlip()
    {
        CardPosition currentPosition = currentCard.cardPosition;
        float random = Random.Range(0f, 1f);
        proceedCaroutine = false;
        bool hasPlayedSFX = false;

        float startRotation = (random < 0.5) ? 0f : 180f;
        float targetRotation = (random < 0.5) ? 180f : 0f;

        for(float i = startRotation; Mathf.Abs(i - targetRotation) > 0.01f; i += (random < 0.5) ? 10f : -10f)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);

            if(!hasPlayedSFX && Mathf.Abs(i - 90) < 10f)
            {
                hasPlayedSFX = true;
                if(audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.cardFlipSound);
                }
            }

            if(i == 90)
            {
                currentCard.cardPosition = (random < 0.5) ? CardPosition.Down : CardPosition.Up;
                if (currentCard.cardPosition != CardPosition.Down)
                {
                    currentCardUI.frontCardName.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    currentCardUI.frontCardDescription.transform.rotation = Quaternion.Euler(0f, 90f, 0f);

                }
                else if (currentCard.cardPosition != CardPosition.Up)
                {
                    currentCardUI.backCardName.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                    currentCardUI.backCardDescription.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                }
            }

            yield return new WaitForSeconds(0.01f);
        }

        ResetUIElements(currentPosition);
        proceedCaroutine = true;
    }

    private void ResetUIElements(CardPosition initialPosition)
    {
        currentCardUI.cardHandle.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        currentCardUI.frontNumber.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        currentCardUI.backNumber.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        currentCardUI.frontCardName.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        currentCardUI.frontCardDescription.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        currentCardUI.backCardName.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        currentCardUI.backCardDescription.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    //public void BeginFlip()
    //{
    //    StartCoroutine(FlipCoroutine());
    //}

    public void BeginFlip()
    {
        if (isOverPlayArea)
        {

            if (proceedCaroutine)
            {
                StartCoroutine(OnPlayFlip());
                hasFlipped = true;
            }
        }
    }
}
