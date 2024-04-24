using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FusionUI : MonoBehaviour
{
    [SerializeField]
    private CardUI[] material;

    [SerializeField]
    private CardUI product;

    private FuseData data;

    public FuseData Data
    {
        get
        {
            return data;
        }

        set
        {
            data = value;

            material[0].SetCardData(data.FusionMaterial[0]);
            material[1].SetCardData(data.FusionMaterial[1]);

            product.SetCardData(data.FusionProduct);
        }
    }
}