using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField]
    private Currency gold = new Currency();

    [SerializeField]
    private List<CardSell> cardSell = new List<CardSell>();

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI goldText;

    [SerializeField]
    private List<CardUI> cardSellUi = new List<CardUI>();

    public override void Initialization()
    {
        gold.OnValueChanged.AddListener((a) =>
        {
            goldText.text = $"{a}";

            save();
        });

        setData();

        load();
    }

    private void load()
    {
        if (SaveManager.Instance.IsNewPlayer)
        {
            float startingGold = 1000;
            gold.Set(startingGold);
        }
        else
        {
            gold.Set(SaveManager.Instance.playerData.goldAmount);
        }
    }

    private void save()
    {
        SaveManager.Instance.playerData.goldAmount = gold.CurValue;

        SaveManager.Instance.Save();
    }

    [ContextMenu("Randomize")] // split this to randomize value
    private void setData()
    {
        List<CardData> all = Inventory.Instance.cardBank.AllCards;
        cardSell.Clear();
        for (int i = 0; i < cardSellUi.Count; i++)
        {
            CardData card = all[Random.Range(0, all.Count)]; // SAVE THE SHOP?
            CardSell generated = new CardSell(card, 100);
            cardSell.Add(generated);

            // UI
            cardSellUi[i].SetCardData(cardSell[i].card);
            Button buyBtn = cardSellUi[i].transform.GetChild(1).GetComponent<Button>();

            buyBtn.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = $"Buy {cardSell[i].price}";
            cardSellUi[i].transform.GetChild(1).GetComponent<Button>().interactable = true;

            int copy = i;

            buyBtn.onClick.RemoveAllListeners();
            buyBtn.onClick.AddListener(() =>
            {
                buyBtnAction(copy);
            });
        }
    }

    private void buyBtnAction(int i)
    {
        if (cardSellUi[i].Data != null)
        {
            buy(i);
        }
    }

    public void ProcessReward(RewardData[] rewards)
    {
        foreach (RewardData reward in rewards)
        {
            if (reward.CardReward != null)
            {
                Inventory.Instance.AddNewCard(reward.CardReward);
            }
            else if (reward.Amount > 0)
            {
                gold.Add(reward.Amount);
            }
            
        }

        // random shop
        setData();

        SaveManager.Instance.playerData.goldAmount = gold.CurValue;
    }

    private bool buy(int i)
    {
        CardSell _card = cardSell[i];

        if (gold.CurValue >= _card.price)
        {
            Inventory.Instance.AddNewCard(_card.card);
            gold.Add(-_card.price);

            cardSellUi[i].SetBlank("Sold");
            cardSellUi[i].transform.GetChild(1).GetComponent<Button>().transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = $"Sold";
            cardSellUi[i].transform.GetChild(1).GetComponent<Button>().interactable = false;

            return true;
        }
        else
        {
            Debug.Log("Not enough money");
            return false;
        }
    }

    public void Sell(PointerEventData data)
    {
        if (!data.pointerDrag.TryGetComponent(out CardUI cardUi))
        {
            return;
        }

        if (cardUi.transform.parent.name == "Bag")
        {
            sellCard(cardUi);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Can only sell cards from bag pile");
#endif
        }
    }

    private void sellCard(CardUI cardUi)
    {
        Inventory.Instance.RemoveCard(cardUi);
    }
}

[System.Serializable]
public class CardSell
{
    public CardData card;
    public float price = 100;

    public CardSell(CardData card, int price)
    {
        this.card = card;
        this.price = price;
    }
}