using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Data")]
public class ScriptableCard : ScriptableObject
{
    [field: SerializeField] public string card_Name {  get; private set; }
    [field: SerializeField] public int left_Number { get; private set; }
    [field: SerializeField] public int right_Number { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }

}
