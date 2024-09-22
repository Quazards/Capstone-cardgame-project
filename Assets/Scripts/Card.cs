using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [field: SerializeField]public ScriptableCard cardData { get; private set; }

    public void setUp(ScriptableCard data)
    {
        cardData = data;
        GetComponent<CardUI>().SetCardUI();
    }
}
