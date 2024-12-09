using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [field: SerializeField]public ScriptableCard cardData { get; private set; }

    public CardPosition cardPosition;
    public int storedEnergy = 0;

    [HideInInspector] public CardRotation cardRotation;

    public int tempFrontNumber;
    public int tempBackNumber;
    public bool isBuffed = false;

    private void Awake()
    {
        cardRotation = GetComponent<CardRotation>();

    }

    public void SetUp(ScriptableCard data)
    {
        cardData = data;
        tempBackNumber = data.back_Number;
        tempFrontNumber = data.front_Number;
        GetComponent<CardUI>().SetCardUI();
    }

    public CardType CurrentCardType()
    {
        if (cardPosition == CardPosition.Up) 
        {
            return cardData.front_Type;
        }
        else
        {
            return cardData.back_Type;
        }
    }    

    public void PlayCard(List<Card> allCards)
    {
        foreach(var effect in cardData.card_Effect)
        {
            effect.ApplyEffect(this, allCards);
        }

    }

    public void BuffCard()
    {
        isBuffed = true;
        GetComponent<CardUI>().SetCardUI();
    }

    public void RevertBuff()
    {
        tempFrontNumber = cardData.front_Number;
        tempBackNumber = cardData.back_Number;
        isBuffed = false;

        GetComponent<CardUI>().SetCardUI();
    }

    public void ConsumeEnergy(int amount)
    {
        TurnSystem.Instance.currentEnergy -= amount;
        storedEnergy += amount;
    }

    public void RefundEnergy(int amount)
    {
        int refundedEnergy = Mathf.Clamp(storedEnergy, 0, amount);
        TurnSystem.Instance.currentEnergy += refundedEnergy;
        storedEnergy -= refundedEnergy;
    }
}
