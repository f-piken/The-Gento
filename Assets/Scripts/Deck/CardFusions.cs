using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFusions : Singleton<CardFusions>
{
    [SerializeField]
    private FusionData FuseDatas;

    private Coroutine fusingRoutine;

    #region UI

    [Header("UI")]
    [SerializeField]
    private Transform parentUi;

    [SerializeField]
    private GameObject fusionUiPrefab;

    [SerializeField]
    private List<FusionUI> fusionUi = new List<FusionUI>();

    #endregion UI

    private CardData fuse(CardData a, CardData b)
    {
        foreach (FuseData data in FuseDatas.Datas)
        {
            if ((a == data.FusionMaterial[0] && b == data.FusionMaterial[1]) ||
                (a == data.FusionMaterial[1] && b == data.FusionMaterial[0]))
            {
                return data.FusionProduct;
            }
        }

        return null;
    }

    public void Fusion(CardUI a, CardUI b)
    {
        fusingRoutine = StartCoroutine(fusingYield(a, b));
    }

    private IEnumerator fusingYield(CardUI a, CardUI b)
    {
        CardData newCard = fuse(a.Data, b.Data);

        if (newCard != null)
        {
            GameManager.Instance.Deck.AddToGrave(a);
            GameManager.Instance.Deck.AddToGrave(b);

            b.SetBlank();

            // FX
            LeanTween.scale(a.gameObject, Vector3.one * 1.5f, 0.2f).setLoopPingPong(1).setEaseOutQuint();
            yield return new WaitForSeconds(0.4f);
            AudioManager.Instance.PlaySfx(2);

            GameObject vfx = ObjectPool.Instance.Spawn("VFX_DisintegrateUI", a.transform);
            LeanTween.delayedCall(vfx, 0.5f, () =>
            {
                ObjectPool.Instance.BackToPool(vfx);
            });

            a.SetCardData(newCard, true);
        }
    }

    #region UI

    public void LoadFusionList()
    {
        StartCoroutine(loadFusionYield());
    }

    private IEnumerator loadFusionYield()
    {
        if (fusionUi.Count > 0)
        {
            for (int i = 0; i < fusionUi.Count; i++)
            {
                Destroy(fusionUi[i].gameObject);
            }

            fusionUi.Clear();
        }

        for (int i = 0; i < FuseDatas.Datas.Length; i++)
        {
            FusionUI spawn = Instantiate(fusionUiPrefab, parentUi).GetComponent<FusionUI>();
            spawn.Data = FuseDatas.Datas[i];
            fusionUi.Add(spawn);
        }

        yield return null;
    }

    #endregion UI
}