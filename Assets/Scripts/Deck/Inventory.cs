using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : Singleton<Inventory>
{
    [SerializeField]
    private CardPile dBag;

    //[SerializeField]
    //private CardPile dTrunk;

    [SerializeField]
    private CardPile dDeck;

    [Header("CardBank")]
    public CardBank cardBank;

    public bool DeckIsFine
    {
        get
        {
            if (dDeck.cards.Count == 10)
            {
                StartCoroutine(saveDeck());
                return true;
            }
            else
            {
                Debug.Log("Deck is bad");
                return false;
            }
        }
    }

    public List<CardData> DeckCards
    {
        get
        {
            return dDeck.cards;
        }
    }

    //public List<CardData> TrunkCards
    //{
    //    get
    //    {
    //        return dTrunk.cards;
    //    }
    //}

    [SerializeField]
    private int lastSort;

    [Header("UI")]
    public GameObject cardUiPrefab;

    [SerializeField]
    private Button backBtn;

    [SerializeField]
    private Button[] sortBtn;

    public override void Initialization()
    {
        dBag.type = PileType.bag;
        dDeck.type = PileType.deck;
        //dTrunk.type = PileType.trunk;

        StartCoroutine(Initialize());

        //backBtn.onClick.AddListener(() => BackMainMenu());
        for (int i = 0; i < sortBtn.Length; i++)
        {
            int copy = i + 1;
            sortBtn[i].onClick.AddListener(() => Sort(copy));
        }
    }

    public IEnumerator Initialize()
    {
        yield return loadDeck();

        yield return spawn(dBag);
        yield return spawn(dDeck);
        //yield return spawn(dTrunk);
    }

    private IEnumerator loadDeck()
    {
        // Load Card

        //bag.cards.AddRange(cardBank.DefaultCard);

        if (SaveManager.Instance.IsNewPlayer)
        {
            dDeck.cards.AddRange(cardBank.DefaultCard);
            //dTrunk.cards.AddRange(cardBank.TrunkCards);
            yield return saveDeck();
        }
        else
        {
            dDeck.cards.AddRange(cardBank.GetCards(SaveManager.Instance.playerData.cardSave.DeckCards));
            //dTrunk.cards.AddRange(cardBank.GetCards(SaveManager.Instance.playerData.cardSave.TrunkCards));
            dBag.cards.AddRange(cardBank.GetCards(SaveManager.Instance.playerData.cardSave.BagCards));
        }

        yield return null;
    }

    private IEnumerator saveDeck()
    {
        List<string> dDeckSave = new List<string>();
        foreach (CardData item in dDeck.cards)
        {
            dDeckSave.Add(item.name);
        }

        SaveManager.Instance.playerData.cardSave.DeckCards = dDeckSave;

        //List<string> dTrunkSave = new List<string>();
        //foreach (CardData item in dTrunk.cards)
        //{
        //    dTrunkSave.Add(item.name);
        //}

        //SaveManager.Instance.playerData.cardSave.TrunkCards = dTrunkSave;

        List<string> dBagSave = new List<string>();
        foreach (CardData item in dBag.cards)
        {
            dBagSave.Add(item.name);
        }

        SaveManager.Instance.playerData.cardSave.BagCards = dBagSave;

        SaveManager.Instance.Save();

        yield return null;
    }

    #region UI

    private IEnumerator spawn(CardPile pile)
    {
        for (int i = 0; i < pile.cards.Count; i++)
        {
            CardUI spawn = Instantiate(cardUiPrefab, pile.pivotCard).GetComponent<CardUI>();

            pile.cardUI.Add(spawn);
            spawn.SetCardData(pile.cards[i]);
        }

        yield return null;
    }

    #endregion UI

    #region Drag Drop Card

    public void Swap(CardUI a, CardUI b)
    {
        Debug.Log($"Dragged: {a.Data.name}: - {b.Data.name}:");

        // sfx
        AudioManager.Instance.PlaySfx(0);

        CardData c = a.Data;

        a.SetCardData(b.Data);
        b.SetCardData(c);

        // SWAP SYSTEM
        StartCoroutine(refreshAllPile());
    }

    private IEnumerator refreshAllPile()
    {
        yield return dBag.RefreshCardData();
        yield return dDeck.RefreshCardData();
        //yield return dTrunk.Refresh();
    }

    #endregion Drag Drop Card

    public void AddNewCard(CardData a)
    {
        Debug.Log($"Added: {a.name}");
        dBag.cards.Insert(0, a);

        CardUI newCard = Instantiate(cardUiPrefab, dBag.pivotCard).GetComponent<CardUI>();
        newCard.SetCardData(a);
        newCard.transform.SetAsFirstSibling();
        dBag.cardUI.Add(newCard);

        SaveManager.Instance.playerData.cardSave.BagCards.Insert(0, a.name);
        save();

        //sfx
        AudioManager.Instance.PlaySfx(0);
    }

    public void RemoveCard(CardUI a)
    {
        dBag.cards.Remove(a.Data);

        SaveManager.Instance.playerData.cardSave.BagCards.Remove(a.Data.name);

        dBag.cardUI.Remove(a);
        Destroy(a.gameObject);

        save();

        StartCoroutine(dBag.RefreshCardUi());

        //sfx
        AudioManager.Instance.PlaySfx(2);
    }

    private void save()
    {
        SaveManager.Instance.Save();
    }

    #region Sort

    public void Sort(int index)
    {
        /* 1 -1 Name
         * 2 -2 Element
         * 3 -3 Type
         * 4 -4 DamageTotal ?
         */
        if (lastSort / index == 1)
        {
            index *= -1;
        }

        lastSort = index;

        StartCoroutine(SortNumerator(lastSort));

        // sfx
        AudioManager.Instance.PlaySfx(0);
    }

    private IEnumerator SortNumerator(int index)
    {
        yield return dDeck.Sort(index);
        yield return dBag.Sort(index);
    }

    #endregion Sort

    public void BackMainMenu()
    {
    }
}

[System.Serializable]
public class CardPile
{
    public PileType type;
    public List<CardUI> cardUI = new List<CardUI>();

    public List<CardData> cards = new List<CardData>();

    [Header("Components")]
    public Transform pivotCard;

    public IEnumerator RefreshCardData()
    {
        cards.Clear();
        for (int i = 0; i < cardUI.Count; i++)
        {
            cards.Add(cardUI[i].Data);
        }
        yield return null;
    }

    public IEnumerator RefreshCardUi()
    {
        for (int i = 0; i < cardUI.Count; i++)
        {
            cardUI[i].SetCardData(cards[i]);
        }
        yield return null;
    }

    public IEnumerator Sort(int index)
    {
        if (index == 1)
        {
            cards.Sort((a, b) => a.name.CompareTo(b.name));
        }
        else if (index == -1)
        {
            cards.Sort((a, b) => b.name.CompareTo(a.name));
        }
        else if (index == 2)
        {
            cards.Sort((a, b) => a.name.CompareTo(b.name));

            cards.Sort((a, b) => a.elemType.CompareTo(b.elemType));
        }
        else if (index == -2)
        {
            cards.Sort((a, b) => b.name.CompareTo(a.name));

            cards.Sort((a, b) => b.elemType.CompareTo(a.elemType));
        }
        else if (index == 3)
        {
            cards.Sort((a, b) => a.name.CompareTo(b.name));

            cards.Sort((a, b) => a.type.CompareTo(b.type));
        }
        else if (index == -3)
        {
            cards.Sort((a, b) => b.name.CompareTo(a.name));

            cards.Sort((a, b) => b.type.CompareTo(a.type));
        }

        yield return RefreshCardUi();
    }
}

public enum PileType { bag, deck, trunk }