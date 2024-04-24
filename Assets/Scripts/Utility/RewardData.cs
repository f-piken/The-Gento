using System.Collections;
using UnityEngine;

[System.Serializable]
public class RewardData
{
    [Header("Fill this for Gold/Money Reward")]
    public float Amount = 1;

    [Header("Fill this for Card Reward")]
    public CardData CardReward;
}

public enum CurrencyType { gold, diamond, xp, card };