using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBank : MonoBehaviour
{
    [SerializeField]
    private List<CardData> allCards = new List<CardData>();

    public List<CardData> AllCards => allCards;

    [SerializeField]
    private List<CardData> defaultCards = new List<CardData>();

    public List<CardData> DefaultCard => defaultCards;

    //[SerializeField]
    //private List<CardData> trunkCards = new List<CardData>();
    //public List<CardData> TrunkCards => trunkCards;

    private CardData GetCard(string name)
    {
        CardData x = allCards.Find((a) => a.name == name);

        if (x == null)
        {
            Debug.Log($"Cannot find card with name {name}");
            return null;
        }

        return allCards.Find((a) => a.name == name);
    }

    public List<CardData> GetCards(List<string> names)
    {
        List<CardData> x = new List<CardData>();
        foreach (string name in names)
        {
            x.Add(GetCard(name));
        }

        return x;
    }
}