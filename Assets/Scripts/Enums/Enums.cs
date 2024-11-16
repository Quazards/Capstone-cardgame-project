#region Card Enums
public enum CardType
{
    Attack,
    Defend
}

public enum CardPosition
{
    Up,
    Down
}

public enum CardOwnership
{
    Player,
    Enemy
}

public enum CardRarity
{
    Common,
    Uncommon,
    Rare
}

#endregion

#region Enemy Enums

public enum EnemyType
{
    Regular,
    Miniboss,
    Boss
}

#endregion

#region Other

public enum CombatPhase
{
    TurnStart,
    TurnEnd,
    PlayerWin,
    PlayerLose,
    CombatStart
}

#endregion