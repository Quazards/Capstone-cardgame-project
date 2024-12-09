using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CardUI : MonoBehaviour
{
    private Card card;

    //[Header("Prefab elements")]
    public Image front_CardImage;
    public Image back_CardImage;
    public Image cardTypeBackground;
    public Image cardHandle;

    public TextMeshProUGUI frontCardName;
    public TextMeshProUGUI backCardName;
    public TextMeshProUGUI frontNumber;
    public TextMeshProUGUI backNumber;
    public TextMeshProUGUI frontCardDescription;
    public TextMeshProUGUI backCardDescription;

    [SerializeField] private Sprite commonAttackBackground;
    [SerializeField] private Sprite uncommonAttackBackground;
    [SerializeField] private Sprite rareAttackBackground;
    [SerializeField] private Sprite commonDefendBackground;
    [SerializeField] private Sprite uncommonDefendBackground;
    [SerializeField] private Sprite rareDefendBackground;
    

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
            SetTextBasedOnPosition();
            SetCardImage();
            SetTypeBackground();
        }
    }

    private void SetCardText()
    {
        frontCardName.text = card.cardData.card_Name;
        backCardName.text = card.cardData.card_Name;
        frontNumber.text = card.tempFrontNumber.ToString();
        backNumber.text = card.tempBackNumber.ToString();
         
        if (card.cardData.card_Description == null)
            return;

        frontCardDescription.text = card.cardData.card_Description;
        backCardDescription.text= card.cardData.card_Description;
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
                    cardTypeBackground.sprite = GetRarityBackground(card.cardData.card_Rarity, true);
                    break;
                case CardType.Defend:
                    cardTypeBackground.sprite = GetRarityBackground(card.cardData.card_Rarity, false); 
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
                    cardTypeBackground.sprite = GetRarityBackground(card.cardData.card_Rarity, true);
                    break;
                case CardType.Defend:
                    cardTypeBackground.sprite = GetRarityBackground(card.cardData.card_Rarity, false);
                    break;
            }
        }
    }

    private Sprite GetRarityBackground(CardRarity rarity, bool isAttack)
    {
        switch (rarity)
        {
            case CardRarity.Common:
                return isAttack ? commonAttackBackground : commonDefendBackground;
            case CardRarity.Uncommon:
                return isAttack ? uncommonAttackBackground : uncommonDefendBackground;
            case CardRarity.Rare:
                return isAttack ? rareAttackBackground : rareDefendBackground;
            default:
                return isAttack ? commonAttackBackground: commonDefendBackground;
        }
    }

    private void SetTextBasedOnPosition()
    {
        if(card.cardPosition == CardPosition.Up)
        {
            frontNumber.gameObject.SetActive(true);
            frontCardName.gameObject.SetActive(true);
            frontCardDescription.gameObject.SetActive(true);

            backNumber.gameObject.SetActive(false);
            backCardName.gameObject.SetActive(false);
            backCardDescription.gameObject.SetActive(false);
        }
        else if(card.cardPosition == CardPosition.Down)
        {
            backNumber.gameObject.SetActive(true);
            backCardName.gameObject.SetActive(true);
            backCardDescription.gameObject.SetActive(true);

            frontNumber.gameObject.SetActive(false);
            frontCardName.gameObject.SetActive(false);
            backCardDescription.gameObject.SetActive(false);
        }
    }

    private bool isFrontAttack(ScriptableCard card)
    {
        if (card.front_Type == CardType.Attack) return true;
        else return false;
    }

    public void SetCardData(ScriptableCard cardData)
    {
        if (cardData == null)
        {
            return;
        }

        // Set the card text and images
        cardTypeBackground.sprite = GetRarityBackground(cardData.card_Rarity, isFrontAttack(cardData));

        frontCardName.text = cardData.card_Name;
        backCardName.text = cardData.card_Name;
        frontNumber.text = cardData.front_Number.ToString();
        backNumber.text = cardData.back_Number.ToString();
        front_CardImage.sprite = cardData.front_Image;
        back_CardImage.sprite = cardData.back_Image;

        if (cardData.card_Description == null)
            return;

        frontCardDescription.text = cardData.card_Description;
        backCardDescription.text = cardData.card_Description;

    }
}
