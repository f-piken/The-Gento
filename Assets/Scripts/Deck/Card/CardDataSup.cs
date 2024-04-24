using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sup Card", menuName = "Salwa/Sup Card")]

public class CardDataSup : CardData
{
    [Header("Sup Stats")]
    public float healAmount = 0;

    public override bool hasEffect
    {
        get
        {
            return base.hasEffect && (healAmount != 0 || buffData.hasBuffEffect);
            //return healAmount != 0 || buffData.hasBuffEffect;
        }
    }

    public CardDataSup()
    {
        type = CardType.Sup;
    }

    public override void Action(BaseChar target)
    {
        base.Action(target);

        AudioManager.Instance.PlaySfx(2);

        target.IncreaseHealth(healAmount); // set sound, set sfx prefab
    }
}