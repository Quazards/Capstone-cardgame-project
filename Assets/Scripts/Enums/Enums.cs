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

public enum CardEffectType
{
    Energy,
    Draw,
    Buff,
    Damage,
    Heal
}

public enum CardTarget
{
    Player,
    Self,
    Enemy
}

public enum CardTriggerTime
{
    StartOfTurn,
    OnActivation
}

public enum CardTriggerCondition
{
    None,
    AlliedAttackCard,
    AlliedDefendCard
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
    CombatStart,
    CardKeep
}

#endregion