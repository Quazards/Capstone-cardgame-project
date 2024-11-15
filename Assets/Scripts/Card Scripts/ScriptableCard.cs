using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card Data")]
public class ScriptableCard : ScriptableObject
{
    [field: SerializeField] public string card_Name {  get; private set; }
    [field: SerializeField] public int front_Number { get; private set; }
    [field: SerializeField] public int back_Number { get; private set; }
    [field: SerializeField] public Sprite front_Image { get; private set; }
    [field: SerializeField] public Sprite back_Image { get; private set; }
    [field: SerializeField] public CardType front_Type { get; private set; }
    [field: SerializeField] public CardType back_Type { get; private set; }
    [field: SerializeField] public CardOwnership card_Ownership { get; private set; }


}
