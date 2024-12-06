using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardEffect
{

    public CardEffectType effectType;
    public int value;
    public CardTarget target;
    public CardTriggerTime triggerTime;
    public CardTriggerCondition triggerCondition;
    public int duration;

    public void ApplyEffect(Card currentCard, List<Card> cardList)
    {
        switch(triggerTime)
        {
            case CardTriggerTime.OnActivation:
                ExecuteEffect(currentCard, cardList);
                break;
            case CardTriggerTime.StartOfTurn:
                TurnSystem.Instance.RegisterEffect(this, currentCard, cardList, duration);
                break;
        }
    }

    public void ExecuteEffect(Card currentCard, List<Card> cardList)
    {
        switch (effectType)
        {
            case CardEffectType.Buff:
                ApplyBuff(currentCard, cardList);
                break;
            case CardEffectType.Energy:
                ApplyEnergy(cardList, currentCard);
                break;
            case CardEffectType.Draw:
                ApplyDraw(cardList, currentCard);
                break;
            case CardEffectType.Damage:
                ApplyDamage(value);
                break;
            case CardEffectType.Heal:
                ApplyHeal(value);
                break;
        }
    }

    private void ApplyBuff(Card currentCard, List<Card> cardList)
    {
        switch(target)
        {
            case CardTarget.Self:

                if (CheckTriggerCondition(cardList, currentCard))
                {

                    if (currentCard.cardPosition == CardPosition.Up)
                    {
                        currentCard.BuffCard();
                        int newValue = currentCard.cardData.front_Number * value;
                        currentCard.tempFrontNumber = newValue;
                    }
                    else
                    {
                        currentCard.BuffCard();
                        int newValue = currentCard.cardData.back_Number * value;
                        currentCard.tempBackNumber = newValue;
                    }
                }
                break;
            default:
                return;
        }
    }

    private void ApplyEnergy(List<Card> cardList, Card currentCard)
    {
        if(CheckTriggerCondition(cardList, currentCard))
        {
            TurnSystem.Instance.currentEnergy += value;
        }
    }

    private void ApplyDraw(List<Card> cardList, Card currentCard)
    {
        switch(target)
        {
            case CardTarget.Player:
                if (CheckTriggerCondition(cardList, currentCard))
                {
                    TurnSystem.Instance.playerDeck.AnyAmountDraw(value);
                }
                break;
            case CardTarget.Enemy:
                if (CheckTriggerCondition(cardList, currentCard))
                {
                    TurnSystem.Instance.enemyDeck.AnyAmountDraw(value);
                }
                break;
            default:
                return;
        }
    }

    private void ApplyDamage(int  damage)
    {
        switch(target)
        {
            case CardTarget.Player:
                CombatManager.Instance.DealDamageToPlayer(damage);
                break;
            case CardTarget.Enemy:
                CombatManager.Instance.DealDamageToEnemy(damage);
                break;
            default :
                return;
        }
    }

    private void ApplyHeal(int amount)
    {
        switch(target)
        {
            case CardTarget.Player:
                CombatManager.Instance.HealPlayer(amount);
                break;
            case CardTarget.Enemy:
                CombatManager.Instance.HealEnemy(amount);
                break;
            default :
                return;
        }
    }

    private bool CheckTriggerCondition(List<Card> cardlist, Card currentCard)
    {
        if(triggerCondition == CardTriggerCondition.None)
        {
            return true;
        }
        else if(triggerCondition == CardTriggerCondition.AlliedAttackCard)
        {
            int otherAttackCards = 0;

            foreach (Card card in cardlist)
            {
                CardType currentType = card.CurrentCardType();

                if (card != currentCard && card.cardData.card_Ownership == CardOwnership.Player)
                {
                    if (currentType == CardType.Attack)
                    {
                        otherAttackCards++;
                    }
                }
            }

            if (otherAttackCards != 0)
                return true;
        }
        else if (triggerCondition == CardTriggerCondition.AlliedDefendCard)
        {
            int otherDefendCards = 0;

            foreach (Card card in cardlist)
            {
                CardType currentType = card.CurrentCardType();

                if (card != currentCard && card.cardData.card_Ownership == CardOwnership.Player)
                {
                    if (currentType == CardType.Defend)
                    {
                        otherDefendCards++;
                    }
                }
            }

            if(otherDefendCards != 0)
                return true;
        }

        return false;
    }
}
