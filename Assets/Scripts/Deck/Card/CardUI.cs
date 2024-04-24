using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class CardUI : DragAndDrop
{
    [SerializeField]
    private CardData data;
    public CardData Data => data;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image border;

    [SerializeField]
    private Image elemImage;

    [SerializeField]
    private TextMeshProUGUI cardName;

    [SerializeField]
    private TextMeshProUGUI damage;

    [SerializeField]
    private Button cardBtn;

    private bool isFused;
    public bool IsFused => isFused;

    public void SetCardData(CardData _data, bool Fused = false)
    {
        data = _data;
        if (data != null) 
        {
            cardName.text = data.name; 
            icon.sprite = data.sprite;
            icon.color = Color.white;

            border.color = CardDetailHandler.Instance.typeColor[(int)data.type];
            elemImage.sprite = CardDetailHandler.Instance.elemSprite[(int)data.elemType];
            isFused = Fused;

            if (data is CardDataAtk)
            {
                CardDataAtk atkData = (CardDataAtk)data;
                damage.text = $"{ atkData.totalDamage}";
            }
            else
            {
                CardDataSup atkData = (CardDataSup)data;
                damage.text = $"{ atkData.healAmount}";
            }

            cardBtn.onClick.RemoveAllListeners();
            cardBtn.onClick.AddListener(() =>
            {
                if (data != null)
                    CardDetailHandler.Instance.ViewData = data;
            });
        }
    }

    public void SetBlank(string blankText = "")
    {
        cardName.text = blankText;

        data = null;
        isFused = false;

        icon.color = new Color(0, 0, 0, 0);
        border.color = new Color(0, 0, 0, 0);
    }

    internal override void Start()
    {
        base.Start();
    }

    public override void OnDropActionWorld()
    {
        base.OnDropActionWorld();

        Transform target = RaycastHandler.Instance.RayObject;

        if (target != null && data != null)
        {
            // get target in parent
            int cardIndex = transform.GetSiblingIndex();
            GameManager.Instance.Level.Player.DeckAttacking(target.parent.GetComponent<BaseChar>(), this, cardIndex);
        }
    }

    public override void OnDropAlike(DragAndDrop _draggedSlot)
    {
        base.OnDropAlike(_draggedSlot);

        if (_draggedSlot != null && _draggedSlot.TryGetComponent(out CardUI ui))
        {
            if (GameManager.Instance.IsInGame)
            {
                CardFusions.Instance.Fusion(this, ui);
            }
            else
            {
                Inventory.Instance.Swap(this, ui);
            }
        }
    }
}