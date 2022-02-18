using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAppObject : MonoBehaviour
{
    public AppState targetState;
    Vector2 ScreenSize;

    RectTransform Rtransform;

    static float UISpeed = 0.25f;

    private void Start()
    {
        Rtransform = GetComponent<RectTransform>();

        ScreenSize = new Vector2(Screen.width, Screen.height);

        if (AppManager.instance.CurrenState == targetState)
        {
            Open(true);
        }
        else Close(true);

        AppManager.instance.OnStateEnter.AddListener(TryOpen);
        AppManager.instance.OnStateExit.AddListener(TryClose);
    }

    void TryOpen(AppState state, AppState lastState)
    {
        bool isBiggerPriority = (int)state > (int)lastState;
        if (state == targetState) Open(isBiggerPriority);
    }

    void TryClose(AppState state, AppState lastState)
    {
        bool isBiggerPriority = (int)state > (int)lastState;
        if (state == targetState) Close(isBiggerPriority);
    }

    protected virtual void Open(bool isBiggerPriority)
    {
        Rtransform.anchoredPosition = new Vector2(isBiggerPriority ? ScreenSize.x : ScreenSize.x * -1, 0f)/2;
        Rtransform.DOAnchorPos(Vector2.zero, UIAppObject.UISpeed);
    }

    protected virtual void Close(bool isBiggerPriority)
    {
        Rtransform.anchoredPosition = Vector2.zero;
        Vector2 movePos = new Vector2(isBiggerPriority ? ScreenSize.x : ScreenSize.x * -1, 0f)/2;
        Rtransform.DOAnchorPos(movePos, UIAppObject.UISpeed);
    }
}
