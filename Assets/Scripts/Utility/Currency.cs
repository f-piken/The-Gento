using System.Collections;
using UnityEngine;

[System.Serializable]
public class Currency
{
    [SerializeField]
    private float value;
    public float CurValue => value;

    public UnityEngine.Events.UnityEvent<float> OnValueChanged;

    public void Set(float a)
    {
        value = a;
        OnValueChanged?.Invoke(value);
    }

    public void Add(float a)
    {
        value += a;
        OnValueChanged?.Invoke(value);
    }
}