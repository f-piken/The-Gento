using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : ScriptableObject
{
    [Header("Card Components")]
    public CardType type;
    public ElementType elemType;

    public Sprite sprite = null;

    [TextArea(3, 6)]
    public string penjelasan = $"Penjelasan ";

    [Header("BUFF DATA POP")]
    public BuffCardData buffData;

    [Header("Effect")]
    // public string animationName="atk";
    public string sfxName = "sfx1";
    public string vfxImpactName = "slash1";
    public string videoClipName = "";

    public bool hasVideo
    {
        get
        {
            return !string.IsNullOrEmpty(videoClipName);
        }
    }

    public virtual bool hasEffect
    {
        get
        {
            return true;
        }
    }

    public float TotalDamage
    {
        get
        {
            return 1;
        }
    }

    public virtual void Action(BaseChar target)
    {
        instantBuff(target);

        speedBuff(target);
        accuracyBuff(target);
        evasionBuff(target);
        dpsBuff(target);
    }

    private void dpsBuff(BaseChar target)
    {
        if (buffData.damagePerSecondAmount > 0)
        {
            Buff _buff = new Buff("Regen", BuffType.dps, buffData.buffDuration, buffData.damagePerSecondAmount);
            target.AddBuff(_buff);
        }
        else if (buffData.damagePerSecondAmount < 0)
        {
            Buff _buff = new Buff("Bleed", BuffType.dps, buffData.buffDuration, buffData.damagePerSecondAmount);
            target.AddBuff(_buff);
        }
    }

    private void instantBuff(BaseChar target)
    {
        if (buffData.stun)
        {
            Buff _buff = new Buff("Stun", BuffType.stun, buffData.buffDuration);
            target.AddBuff(_buff);
        }
        if (buffData.cure)
        {
            Buff _buff = new Buff("Cure", BuffType.dispel, buffData.buffDuration);
            target.AddBuff(_buff);
        }
    }

    private void speedBuff(BaseChar target)
    {
        if (buffData.speedAmount > 0)
        {
            Buff _buff = new Buff("Boost", BuffType.speed, buffData.buffDuration, buffData.speedAmount);
            target.AddBuff(_buff);
        }
        else if (buffData.speedAmount < 0)
        {
            Buff _buff = new Buff("Slow", BuffType.speed, buffData.buffDuration, buffData.speedAmount);
            target.AddBuff(_buff);
        }
    }

    private void accuracyBuff(BaseChar target)
    {
        if (buffData.accuracyAmount > 0)
        {
            Buff _buff = new Buff("Precision", BuffType.acc, buffData.buffDuration, buffData.accuracyAmount);
            target.AddBuff(_buff);
        }
        else if (buffData.accuracyAmount < 0)
        {
            Buff _buff = new Buff("Blind", BuffType.acc, buffData.buffDuration, buffData.accuracyAmount);
            target.AddBuff(_buff);
        }
    }

    private void evasionBuff(BaseChar target)
    {
        if (buffData.evasionAmount > 0)
        {
            Buff _buff = new Buff("Swift", BuffType.eva, buffData.buffDuration, buffData.evasionAmount);
            target.AddBuff(_buff);
        }
        else if (buffData.evasionAmount < 0)
        {
            Buff _buff = new Buff("Sloppy", BuffType.eva, buffData.buffDuration, buffData.evasionAmount);
            target.AddBuff(_buff);
        }
    }
}

public enum CardType { Atk, Ult, Sup }

public enum ElementType { Normal, Fire, Watr, Wind }