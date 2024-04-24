using System.Collections;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Fusion", menuName = "Salwa/FusionData")]

public class FusionData : ScriptableObject
{
    public FuseData[] Datas;
}

[System.Serializable]
public class FuseData
{
    public CardData[] FusionMaterial = new CardData[2];

    public CardData FusionProduct;
}