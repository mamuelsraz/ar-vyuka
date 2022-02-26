using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAppPanel : MonoBehaviour
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
            OpenNow();
        }
        else CloseNow();

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
        Rtransform.DOAnchorPos(Vector2.zero, UIAppPanel.UISpeed);
    }

    protected virtual void Close(bool isBiggerPriority)
    {
        Rtransform.anchoredPosition = Vector2.zero;
        Vector2 movePos = new Vector2(isBiggerPriority ? ScreenSize.x : ScreenSize.x * -1, 0f)/2;
        Rtransform.DOAnchorPos(movePos, UIAppPanel.UISpeed);
    }

    protected virtual void CloseNow()
    {
        Rtransform.anchoredPosition = new Vector2(ScreenSize.x * -1, 0f) / 2;
    }

    protected virtual void OpenNow()
    {
        Rtransform.anchoredPosition = Vector2.zero;
    }
}
