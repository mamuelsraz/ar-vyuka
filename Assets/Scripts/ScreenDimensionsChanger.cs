using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenDimensionsChanger : MonoBehaviour
{
    public static ScreenDimensionsChanger instance;
    public static UnityEvent OnScreenChanged;

    private void Awake()
    {
        OnScreenChanged = new UnityEvent();
        instance = this;
    }

    private void OnRectTransformDimensionsChange()
    {
        OnScreenChanged?.Invoke();
    }
}
