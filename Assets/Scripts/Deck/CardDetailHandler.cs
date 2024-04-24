using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDetailHandler : Singleton<CardDetailHandler>
{
    private CardData data;

    public CardData ViewData
    {
        get => data;
        set
        {
            data = value;

            apply();
        }
    }

    [Header("Panel Card kiri")]
    [SerializeField]
    private Image borderCard;

    [SerializeField]
    private Image sprite;

    [SerializeField]
    private TextMeshProUGUI cardJudul;

    [SerializeField]
    private TextMeshProUGUI cardDamage;

    [Header("Panel kanan")]
    [SerializeField]
    private TextMeshProUGUI judul;

    [SerializeField]
    private TextMeshProUGUI penjelasan;

    [SerializeField]
    private TextMeshProUGUI element;

    [Header("komponen")]
    [SerializeField]
    private Canvas panel;

    public Color[] typeColor = new Color[3];
    public Sprite[] elemSprite = new Sprite[4];

    [SerializeField]
    private string[] elemText = new string[3];

    [SerializeField]
    private Button closeBtn;

    private void apply()
    {
        sprite.sprite = data.sprite;
        cardJudul.text = data.name;
        judul.text = data.name;

        borderCard.color = data.hasVideo ? typeColor[1] : typeColor[(int)data.type];
        element.text = elemText[(int)data.elemType];

        penjelasan.text = data.penjelasan;

        if (data is CardDataAtk)
        {
            CardDataAtk atkData = (CardDataAtk)data;
            cardDamage.text = $"{ atkData.totalDamage}";
        }
        else
        {
            CardDataSup atkData = (CardDataSup)data;
            cardDamage.text = $"{ atkData.healAmount}";
        }

        panel.gameObject.SetActive(true);
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }
}