using System.Collections;
using UnityEngine;

[System.Serializable]
public class Stats
{
    [SerializeField]
    private float value;
    public float CurValue => value;

    private float defaultValue;
    public float DefaultValue => defaultValue;
    public float DeltaValue => value - defaultValue;

    public void Set(float a, bool isDefault = false)
    {
        if (isDefault)
        {
            defaultValue = a;
        }
        value = a;
    }

    public void Add(float a)
    {
        value += a;
    }

    public void Reset()
    {
        value = defaultValue;
    }
}