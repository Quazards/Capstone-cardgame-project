using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card List")]
public class CardPile : ScriptableObject
{
    [field:SerializeField] public List<ScriptableCard> cardsInPile {  get; private set; }

    public void RemoveCardFromPile(ScriptableCard card)
    {
        if(cardsInPile.Contains(card))
        {
            cardsInPile.Remove(card);
        }
        else
        {
            return;
        }
    }

    public void AddCardToPile(ScriptableCard card)
    {
        cardsInPile.Add(card);
    }
}
