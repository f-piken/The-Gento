using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChar : BaseChar
{
    public List<EnemyAtk> enemyAtk = new List<EnemyAtk>();

    private CardData choseAtk
    {
        get
        {
            for (int i = enemyAtk.Count; i-- > 0;)
            {
                if (enemyAtk[i].IsReady)
                {
                    resetAtkCooldown(enemyAtk[i]);

                    return enemyAtk[i].card;
                }
            }

            return null;
        }
    }

    public override void SetData(CharacterData _data)
    {
        base.SetData(_data);

        for (int i = 0; i < _data.atk.Count; i++)
        {
            EnemyAtk _atk = new EnemyAtk(_data.atk[i].card, _data.atk[i].coolDown);
            enemyAtk.Add(_atk);
        }
    }

    public override void TurnPhase()
    {
        base.TurnPhase();

        reduceAtkCooldown();

        BaseChar target = GameManager.Instance.Level.Player;
        CardData curAttack = choseAtk;

        Attacking(target, curAttack);
    }

    private void reduceAtkCooldown()
    {
        foreach (EnemyAtk atk in enemyAtk)
        {
            if (atk.coolDown > 0)
                atk.coolDown--;
        }
    }

    private void resetAtkCooldown(EnemyAtk enemyAtk)
    {
        EnemyAtk defData = data.atk.Find((a) => a.card == enemyAtk.card);
        enemyAtk.coolDown = defData.coolDown;
    }

    public override void Die()
    {
        base.Die();
        GameManager.Instance.Level.WinCheck();
    }

    public override void FinishedAnimating()
    {
        base.FinishedAnimating();
        TurnManager.Instance.NextTurn();
    }
}