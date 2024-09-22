using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "cardPile")]
public class CardPile : ScriptableObject
{
    [field:SerializeField] public List<ScriptableCard> cardsInPile {  get; private set; }

    public void removeCardFromPile(ScriptableCard card)
    {
        if(cardsInPile.Contains(card))
        {
            cardsInPile.Remove(card);
        }
        else
        {
            Debug.Log("CardData is not in pile");
        }
    }

    public void addCardToPile(ScriptableCard card)
    {
        cardsInPile.Add(card);
    }
}
