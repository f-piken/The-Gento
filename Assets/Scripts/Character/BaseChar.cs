using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharUI))]
public class BaseChar : MonoBehaviour
{
    [SerializeField]
    protected ElementType elemType;
    public ElementType ElemType => elemType;
    public bool IsDie => isDie;

    protected bool isDie;

    public Stats s_Health;

    public Stats s_Speed;

    public Stats s_Acc;

    public Stats s_Eva;

    public bool IsStunned => isStunned;
    protected bool isStunned;

    [Header("Components")]
    [SerializeField]
    protected Animator anim;

    [SerializeField]
    protected CharUI ui;

    public CharacterData CharData => data;

    [SerializeField]
    protected CharacterData data;

    public UnityEvent DoCardMove;

    [Header("Components")]
    [SerializeField]
    private AttackEffect effects;

    public virtual void SetData(CharacterData _data)
    {
        data = _data;

        gameObject.name = data.name;
        effects.SetCharcter(this);

        elemType = data.elemType;

        s_Health.Set(data.maxHealth, true);

        ui.Initialize();
        ui.SetHealthBar(s_Health.CurValue / s_Health.DefaultValue);
        isDie = false;

        s_Speed.Set(data.speed, true);
        s_Acc.Set(data.accuracy, true);
        s_Eva.Set(data.evasion, true);

        anim.runtimeAnimatorController = data.anim;
        anim.GetComponent<SpriteRenderer>().color = data.Tint;

        if (data.atk.Count <= 0)
        {
            Debug.LogError($"{data.name} doesn't have any attack move, check his CharacterData!");
        }
    }

    #region BUFF

    public IEnumerator PreTurnBuff()
    {
        yield return effects.PreTurnEffect();
    }

    public IEnumerator PostTurnBuff()
    {
        yield return effects.PostTurnEffect();
    }

    public void AddBuff(Buff buff)
    {
        StartCoroutine(effects.AddEffect(buff));
    }

    public void SetStun(bool state = true)
    {
        isStunned = state;
    }

    public void DispelEffect()
    {
        effects.Dispel();
    }

    #endregion BUFF

    #region ATTACKING

    public virtual void Attacking(BaseChar target, CardData cardData)
    {
        bool willHit = target == this ? true : calculateHitAccuracy(target);

        if (cardData.hasVideo)
        {
            VideoLoader.Instance.PlayDone(cardData.videoClipName, () =>
            {
                hit(target, cardData, willHit);
            });
        }
        else
        {
            anim.Play("atk");
            DoCardMove.AddListener(() =>
            {
                hit(target, cardData, willHit);
            });
        }
    }

    protected virtual void hit(BaseChar target, CardData cardData, bool willHit)
    {
        if (!willHit)
        {
            //ui.SetFloatingText("Miss");
            target.ui.SetFloatingText("Evade");
            return;
        }

        cardData.Action(target);
    }

    private bool calculateHitAccuracy(BaseChar target)
    {
        float chance = s_Acc.CurValue * (100 - target.s_Eva.CurValue) / 100;
        bool isHit = Random.Range(0f, 100f) <= chance;
        return isHit;
    }

    // called on empty start animation
    // RANGED ATTACK TOO
    public virtual void FinishedAnimating()
    {
        DoCardMove?.Invoke();

        DoCardMove.RemoveAllListeners();
    }

    #endregion ATTACKING

    public virtual void TurnPhase()
    {
    }

    public virtual void TakeDamage(CardDataAtk atkCard)
    {
        StartCoroutine(takeDamageRoutine(atkCard));
    }

    private IEnumerator takeDamageRoutine(CardDataAtk atkCard)
    {
        float mult = effectiveness(atkCard.elemType, ElemType);

        float splitDamage = atkCard.lastDamageIncrease ? atkCard.totalDamage / (atkCard.totalAtk + 1) : atkCard.totalDamage / atkCard.totalAtk;

        for (int i = 0; i < atkCard.totalAtk; i++)
        {
            if (i == atkCard.totalAtk - 1 && atkCard.lastDamageIncrease)
            {
                splitDamage *= 2;
            }

            IncreaseHealth(-(splitDamage * mult));

            // Effect
            AudioManager.Instance.PlaySfx(3);
            VfxHurt("VFX_Slash0");

            yield return new WaitForSeconds(0.2f);
        }
    }

    public virtual void IncreaseHealth(float _amount)
    {
        if (isDie || _amount == 0) { return; }
        s_Health.Add(_amount);
        s_Health.Set(Mathf.Clamp(s_Health.CurValue, 0, s_Health.DefaultValue));

        ui.SetHealthBar(s_Health.CurValue / s_Health.DefaultValue);
        ui.SetFloatingText(_amount);

        if (s_Health.CurValue <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
        isDie = true;

        AudioManager.Instance.PlaySfx(4);

        TurnManager.Instance.FindCharUi(this).gameObject.SetActive(false);
        //TurnManager.Instance.RemoveTurn(this);
    }

    public virtual void VfxHurt(string vfxName)
    {
        GameObject vfx = ObjectPool.Instance.Spawn(vfxName, this.transform);
        LeanTween.delayedCall(vfx, 0.5f, () =>
        {
            ObjectPool.Instance.BackToPool(vfx);
        });
    }

    private float effectiveness(ElementType user, ElementType target)
    {
        if (
            (user == ElementType.Fire && target == ElementType.Wind) ||
            (user == ElementType.Wind && target == ElementType.Watr) ||
            (user == ElementType.Watr && target == ElementType.Fire))
        {
            return 1.5f;
        }
        else if (
            (user == ElementType.Wind && target == ElementType.Fire) ||
            (user == ElementType.Watr && target == ElementType.Wind) ||
            (user == ElementType.Fire && target == ElementType.Watr))
        {
            return 0.5f;
        }

        return 1f;
    }
}