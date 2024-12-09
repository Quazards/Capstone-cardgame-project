using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEvents : MonoBehaviour
{
    public static event Action<int> OnPlayerDamageTaken;
    public static event Action<int> OnEnemyDamageTaken;

    public static void InvokeOnPlayerDamageTaken(int amount)
    {
        OnPlayerDamageTaken?.Invoke(amount);
    }

    public static void InvokeOnEnemyDamageTaken(int amount)
    {
        OnEnemyDamageTaken?.Invoke(amount);
    }
}
