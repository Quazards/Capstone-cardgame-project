using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [field: SerializeField] public string enemyName { get; private set; }
    [field: SerializeField] public int maxHealth { get; private set; }
    [field: SerializeField] public CardPile deckList { get; private set; }
    [field: SerializeField] public EnemyType enemyType {  get; private set; } 
    [field: SerializeField] public int enemyDrawAmount { get; private set; }
}
