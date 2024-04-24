using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [Tooltip("should reference the canvas scaler to drag and drop")]
    private Canvas canvas;

    private Image draggedImage;
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    private int orderDefault = 1;
    private int orderTop = 5;

    private bool dropToWorld
    {
        get => !RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition);
    }

    internal virtual void Start()
    {
        canvas = this.GetComponent<Canvas>();

        orderDefault = canvas.sortingOrder;

        canvasGroup = GetComponent<CanvasGroup>();

        if (transform.childCount > 0)
        {
            draggedImage = transform.GetChild(0).GetComponent<Image>();
            rect = draggedImage.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("DRAG N DROP : Has no child image");
        }
    }

    #region drag n drop

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        rect.anchoredPosition = Vector3.zero;
        draggedImage.raycastTarget = true;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        canvas.sortingOrder = orderDefault;

        if (dropToWorld)
        {
            OnDropActionWorld();
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        draggedImage.raycastTarget = false;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        canvas.sortingOrder = orderTop;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        OnDropAlike(eventData.pointerDrag.GetComponent<DragAndDrop>());
    }

    public virtual void OnDropAlike(DragAndDrop _draggedSlot)
    {
#if UNITY_EDITOR
        //Debug.Log($"DRAG N DROP : {this} dragged to {_draggedSlot}");
#endif
    }

    public virtual void OnDropActionWorld()
    {
#if UNITY_EDITOR
        //Debug.Log($"DRAG N DROP : {this} dragged to WORLD");
#endif
    }

    #endregion drag n drop
}