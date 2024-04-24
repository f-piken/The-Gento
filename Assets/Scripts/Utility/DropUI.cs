using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DropUI : MonoBehaviour, IDropHandler
{
    public UnityEvent<PointerEventData> OnDropAction;

    public void OnDrop(PointerEventData eventData)
    {
        OnDropAction?.Invoke(eventData);
    }
}