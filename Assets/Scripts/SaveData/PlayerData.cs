using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string PlayerName;

    public float goldAmount = 0;

    public CardSave cardSave = new CardSave();
}

[System.Serializable]
public class CardSave
{
    public List<string> BagCards = new List<string>();
    public List<string> DeckCards = new List<string>();
    public List<string> TrunkCards = new List<string>();
}