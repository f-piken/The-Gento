using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : Singleton<TimeManager>
{
    public UnityEvent OnTimeProgress;

    [SerializeField]
    private float currentTime;

    private void Update()
    {
        currentTime += Time.deltaTime;
        OnTimeProgress?.Invoke();
    }
}