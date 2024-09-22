using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    private Card card;

    //[Header("Prefab elements")]
    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardBackground;

    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI leftNumber;
    [SerializeField] private TextMeshProUGUI rightNumber;

    private void Awake()
    {
        card = GetComponent<Card>();
        SetCardUI();
    }

    private void OnValidate()
    {
        Awake();
    }

    public void SetCardUI()
    {
        if(card != null && card.cardData != null)
        {
            SetCardText();
            SetCardImage();
        }
    }

    private void SetCardText()
    {
        cardName.text = card.cardData.card_Name;
        leftNumber.text = card.cardData.left_Number.ToString();
        rightNumber.text = card.cardData.right_Number.ToString();
    }

    private void SetCardImage()
    {
        cardImage.sprite = card.cardData.Image;
    }
}
