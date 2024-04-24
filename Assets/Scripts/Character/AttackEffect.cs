using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackEffect
{
    [SerializeField]
    private List<Buff> buffs = new List<Buff>();

    private List<Buff> removedBuff = new List<Buff>();

    private BaseChar user;

    public void SetCharcter(BaseChar _char)
    {
        user = _char;
    }

    public IEnumerator AddEffect(Buff buff)
    {
        // animate buff

        if (isDoubleBuff(buff))
        {
            Buff doubledBuff = buffs.Find((a) => a.mName == buff.mName);

            // only extend the lives
            doubledBuff.mLives = buff.mLives;
        }
        else
        {
            buffs.Add(buff);

            yield return buff.StartEffect(user);
        }
    }

    private bool isDoubleBuff(Buff buff)
    {
        foreach (Buff item in buffs)
        {
            if (item.mName == buff.mName)
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator PreTurnEffect()
    {
        foreach (Buff a in buffs)
        {
            a.mLives--;

            yield return new WaitForSeconds(0.1f);
            yield return a.PreTurnEffect(user);
        }
    }

    public IEnumerator PostTurnEffect()
    {
        foreach (Buff a in buffs)
        {
            if (!a.mStartEffect)
            {
                yield return new WaitForSeconds(0.1f);
                yield return a.PosStartEffect(user);
            }

            if (a.mLives <= 0)
            {
                yield return new WaitForSeconds(0.1f);
                yield return a.FinishEffect(user);
                removedBuff.Add(a);
            }
        }

        buffs.RemoveAll(l => removedBuff.Contains(l));
        removedBuff.Clear();
    }

    public void Dispel()
    {
        foreach (Buff a in buffs)
        {
            if (a.mIsNegative)
            {
                a.FinishEffect(user);
                removedBuff.Add(a);
            }
        }

        buffs.RemoveAll(l => removedBuff.Contains(l));
        removedBuff.Clear();
    }
}

[System.Serializable]
public class Buff
{
    #region DECLARATION

    public string mName;
    public float mAmount = 20f;

    [Header("Stats")]
    public int mLives = 1;
    public BuffType mType;
    public bool mIsNegative;
    public bool mStartEffect;

    public Buff(string name, BuffType type, int lives = 1, float amount = 0)
    {
        mName = name;
        mType = type;
        mLives = lives;
        mAmount = amount;

        if (mType == BuffType.dispel || mType == BuffType.stun)
        {
            mIsNegative = true;
        }
        else
        {
            mIsNegative = amount < 0;
        }
    }

    #endregion DECLARATION

    public IEnumerator StartEffect(BaseChar user)
    {
        switch (mType)
        {
            case BuffType.speed:
                if (!TurnManager.Instance.isCurrentPlaying(user))
                {
                    user.s_Speed.Add(mAmount);

                    yield return TurnManager.Instance.SpeedEffectBuff(user, mAmount);
                    mStartEffect = true;
                }
                break;

            case BuffType.eva:
                user.s_Eva.Add(mAmount);
                mStartEffect = true;

                break;

            case BuffType.acc:
                user.s_Acc.Add(mAmount);
                mStartEffect = true;

                break;

            case BuffType.dispel:
                user.DispelEffect();
                mStartEffect = true;
                break;

            case BuffType.stun:
                user.SetStun();
                mStartEffect = true;

                break;
        }
        //Debug.Log("Start effect");
    }

    public IEnumerator PosStartEffect(BaseChar user)
    {
        switch (mType)
        {
            case BuffType.speed:

                user.s_Speed.Add(mAmount);

                yield return TurnManager.Instance.SpeedEffectBuff(user, mAmount);
                mStartEffect = true;

                break;
        }
    }

    public IEnumerator PreTurnEffect(BaseChar user)
    {
        switch (mType)
        {
            case BuffType.dps:
                user.IncreaseHealth(mAmount);

                break;
        }
        yield return null;
    }

    public IEnumerator FinishEffect(BaseChar user)
    {
        switch (mType)
        {
            case BuffType.speed:
                user.s_Speed.Add(-mAmount);
                yield return TurnManager.Instance.SpeedEffectBuff(user, -mAmount);

                break;

            case BuffType.eva:
                user.s_Eva.Add(-mAmount);
                break;

            case BuffType.acc:
                user.s_Acc.Add(-mAmount);
                break;

            case BuffType.stun:
                user.SetStun(false);

                break;
        }
        //Debug.Log("Finish effect");
    }
}

/*
        switch (mType)
        {
            case BuffType.speed:
                break;

            case BuffType.dps:
                break;

            case BuffType.eva:
                break;

            case BuffType.acc:
                break;

            case BuffType.clear:
                break;

            case BuffType.stun:
                break;

            default:
                break;
        }
        */

public enum BuffType { speed, dps, eva, acc, dispel, stun }