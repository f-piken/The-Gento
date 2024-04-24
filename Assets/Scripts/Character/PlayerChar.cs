using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : BaseChar
{
    public void DeckAttacking(BaseChar target, CardUI cardUi, int cardIndex)
    {
        if (!cardUi.Data.hasEffect)
        {
#if UNITY_EDITOR
            Debug.Log($"{cardUi.Data.name} card has no Effect, also check the Buff Duration ");
#endif
            return;
        }

        Attacking(target, cardUi.Data);
        StartCoroutine(GameManager.Instance.Deck.UsedCard(cardUi, cardIndex));
    }

    protected override void hit(BaseChar target, CardData cardData, bool willHit)
    {
        if (cardData is CardDataAtk)
        {
            CardDataAtk atk = (CardDataAtk)cardData;
            List<BaseChar> targets = GameManager.Instance.Level.GetEnemiesInRange(target, atk.HitArea);

            foreach (BaseChar baseChar in targets)
            {
                if (!baseChar.IsDie)
                    base.hit(baseChar, cardData, willHit);
            }
        }
        else
        {
            base.hit(target, cardData, willHit);
        }
    }

    public override void TurnPhase()
    {
        base.TurnPhase();
        GameManager.Instance.Deck.DeckActive();
    }

    public void EndTurnButton()
    {
        TurnManager.Instance.NextTurn();
        GameManager.Instance.Deck.DeckActive(false);
    }

    public override void Die()
    {
        base.Die();
        GameManager.Instance.Level.LoseCheck();
    }
}