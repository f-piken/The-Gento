using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeckUI : MonoBehaviour
{
    [SerializeField]
    private Image blocker;

    [SerializeField]
    private TextMeshProUGUI topText;

    [SerializeField]
    private Button endButton;

    [Header("Components")]
    [SerializeField]
    private List<CardUI> cardUiList = new List<CardUI>();

    [SerializeField]
    private List<Toggle> cardPadlocks = new List<Toggle>();

    [SerializeField]
    private RectTransform graveStack;

    [SerializeField]
    private RectTransform deckStack;

    private Toggle togle;
    //[SerializeField]
    //private GameObject cardUiPrefab;

    //[SerializeField]
    //private Transform cardUiParent;

    public IEnumerator Initialize()
    {
        endButton.onClick.AddListener(endTurnButton);

        for (int i = 0; i < cardPadlocks.Count; i++)
        {
            int copy = i;
            cardPadlocks[i].onValueChanged.AddListener((bool a) =>
            {
                GameManager.Instance.Deck.LockHandCard(copy, a);
            });
        }

        yield return null;
    }

    public bool CheckFused(int index, out CardUI cardUi)
    {
        cardUi = cardUiList[index];
        return cardUiList[index].IsFused || cardUiList[index].Data == null;
    }

    public IEnumerator applyHandCard(int index, CardData card)
    {
        // draw animation
        yield return new WaitForSeconds(0.1f);

        cardUiList[index].SetCardData(card);

        yield return null;
    }

    public IEnumerator applyUsedCard(int index)
    {
        // deleted animation
        yield return new WaitForSeconds(0.1f);

        cardUiList[index].SetBlank();
        //cardPadlocks[index].gameObject.SetActive(false);

        yield return null;
    }

    public void ClearDeck()
    {
        for (int i = 0; i < cardUiList.Count; i++)
        {
            cardUiList[i].SetBlank();
        }
    }

    public void StateActive(bool _state = true)
    {
        blocker.gameObject.SetActive(!_state);
        endButton.interactable = _state;
    }

    private void endTurnButton()
    {
        GameManager.Instance.Level.Player.EndTurnButton();
    }

    public void SetTopText(string _text)
    {
        topText.text = _text;
    }

    public void UpdateNumber(int deck, int grave)
    {
        deckStack.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{deck}";
        graveStack.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{grave}";
    }

    public void UpdatePadlock(bool condition, bool[] state)
    {
        if (condition)
        {
            for (int i = 0; i < cardPadlocks.Count; i++)
            {
                cardPadlocks[i].gameObject.SetActive(state[i]);
            }
        }
        else
        {
            for (int i = 0; i < cardPadlocks.Count; i++)
            {
                cardPadlocks[i].gameObject.SetActive(true);
            }
        }
    }
}