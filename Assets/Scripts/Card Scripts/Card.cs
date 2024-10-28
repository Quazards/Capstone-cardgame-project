using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [field: SerializeField]public ScriptableCard cardData { get; private set; }
    public CardPosition cardPosition;
    public bool hasUsedPlayEnergy = false;

    public void setUp(ScriptableCard data)
    {
        cardData = data;
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
}
