using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CardUI : MonoBehaviour
{
    private Card card;
    //private CardPosition cardPosition;

    //[Header("Prefab elements")]
    public Image front_CardImage;
    public Image back_CardImage;
    [SerializeField] private Image cardBackground;
    public Image cardTypeBackground;
    public Image cardHandle;

    public TextMeshProUGUI cardName;
    public TextMeshProUGUI frontNumber;
    public TextMeshProUGUI backNumber;

    [SerializeField] private Sprite attackBackground;
    [SerializeField] private Sprite defendBackground;

    private void Awake()
    {
        card = GetComponent<Card>();
        SetCardUI();
    }

    private void OnValidate()
    {
        Awake();
    }

    private void Update()
    {
        SetCardUI();
    }

    public void SetCardUI()
    {
        if(card != null && card.cardData != null)
        {
            SetCardText();
            SetCardImage();
            SetTypeBackground();
        }
    }

    private void SetCardText()
    {
        cardName.text = card.cardData.card_Name;
        frontNumber.text = card.cardData.front_Number.ToString();
        backNumber.text = card.cardData.back_Number.ToString();
    }

    private void SetCardImage()
    {
        if (card.cardPosition == CardPosition.Up)
        {
            front_CardImage.sprite = card.cardData.front_Image;
        }

        else if(card.cardPosition == CardPosition.Down)
        {
            back_CardImage.sprite = card.cardData.back_Image;
        }
    }      


    private void SetTypeBackground()
    {
        if (card.cardPosition == CardPosition.Up)
        {
            frontNumber.gameObject.SetActive(true);
            backNumber.gameObject.SetActive(false);
            switch (card.cardData.front_Type)
            {
                case CardType.Attack:
                    cardTypeBackground.sprite = attackBackground;
                    break;
                case CardType.Defend:
                    cardTypeBackground.sprite = defendBackground;
                    break;
            }
        }
        else if (card.cardPosition == CardPosition.Down)
        {
            frontNumber.gameObject.SetActive(false);
            backNumber.gameObject.SetActive(true);
            switch (card.cardData.back_Type)
            {
                case CardType.Attack:
                    cardTypeBackground.sprite = attackBackground;
                    break;
                case CardType.Defend:
                    cardTypeBackground.sprite = defendBackground;
                    break;
            }
        }
    }

  
}
