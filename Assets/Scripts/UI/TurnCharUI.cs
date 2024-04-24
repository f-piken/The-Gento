using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnCharUI : MonoBehaviour
{
    private BaseChar data;

    public BaseChar Data
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
            gameObject.name = data.name;
            icon.sprite = data.CharData.icon;
            icon.color = data.CharData.Tint;
        }
    }

    public float PosPercent = 0;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI charName;
    public TextMeshProUGUI CharName { get => charName; }

    [SerializeField]
    private Image icon;

    public Image Icon { get => icon; }

    public RectTransform rect
    {
        get => GetComponent<RectTransform>();
    }

    public float curYPos
    {
        get => rect.anchoredPosition.y;
    }
}