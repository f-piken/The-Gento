using System.Collections;

using UnityEngine;

[CreateAssetMenu(fileName = "New Atk Card", menuName = "Salwa/Atk Card")]

public class CardDataAtk : CardData
{
    [Header("Atk Stats", order = 0)]
    public float totalDamage = 100;

    [Tooltip("The total damage will devided into these attacks")]
    [Range(1, 5)]
    public int totalAtk = 1;
    public bool lastDamageIncrease = false;

    [Tooltip("0 for single target, 1 for splash little area, 2 for damage All enemy")]
    [Range(0, 2)]
    public int HitArea = 0;

    public override void Action(BaseChar target)
    {
        base.Action(target);
        target.TakeDamage(this); // set sound, set sfx prefab
    }

    public CardDataAtk()
    {
        type = hasVideo ? CardType.Atk : CardType.Ult;
    }
}