using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/All Enemy")]
public class AllEnemyStatsCollection : ScriptableObject
{
    public EnemyStats[] AllEnemy;
}
