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
    //[SerializeField] private Image cardBackground;
    public Image cardTypeBackground;
    public Image cardHandle;

    public TextMeshProUGUI cardName;
    public TextMeshProUGUI frontNumber;
    public TextMeshProUGUI backNumber;

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
  
    private bool isFrontAttack(ScriptableCard card)
    {
        if (card.front_Type == CardType.Attack) return true;
        else return false;
    }

    public void SetCardData(ScriptableCard cardData)
    {
        if (cardData == null)
        {
            Debug.LogError("cardData is null");
            return;
        }

        Debug.Log($"Setting card data for: {cardData.card_Name}");

        // Set the card text and images
        cardName.text = cardData.card_Name;
        frontNumber.text = cardData.front_Number.ToString();
        backNumber.text = cardData.back_Number.ToString();
        front_CardImage.sprite = cardData.front_Image;
        back_CardImage.sprite = cardData.back_Image;

        cardTypeBackground.sprite = GetRarityBackground(cardData.card_Rarity, isFrontAttack(cardData));
    }
}
