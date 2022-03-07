using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAppPanel : MonoBehaviour
{
    public AppState targetState;
    RectTransform Rtransform;

    public static float UISpeed = 0.25f;

    private void Start()
    {
        Rtransform = GetComponent<RectTransform>();

        if (AppManager.instance.CurrenState == targetState)
        {
            OpenNow();
        }
        else CloseNow();

        AppManager.instance.OnStateEnter.AddListener(TryOpen);
        AppManager.instance.OnStateExit.AddListener(TryClose);

        ScreenDimensionsChanger.OnScreenChanged.AddListener(RePlace);
    }

    void RePlace()
    {
        if (AppManager.instance.CurrenState != targetState)
        {
            Vector2 pos = new Vector2(GetOutOfBoundsPosition(), 0);
            Rtransform.anchoredPosition = pos;
        }
    }

    float GetOutOfBoundsPosition()
    {
        float position;
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight) position = Display.main.systemHeight;
        position = Display.main.systemWidth;
        Debug.Log($"changing pos: {position}");
        return position;
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
        float pos = GetOutOfBoundsPosition();
        Rtransform.anchoredPosition = new Vector2(isBiggerPriority ? pos : pos * -1, 0f) / 2;
        Rtransform.DOAnchorPos(Vector2.zero, UIAppPanel.UISpeed);
    }

    protected virtual void Close(bool isBiggerPriority)
    {
        float pos = GetOutOfBoundsPosition();
        Rtransform.anchoredPosition = Vector2.zero;
        Vector2 movePos = new Vector2(isBiggerPriority ? pos : pos * -1, 0f) / 2;
        Rtransform.DOAnchorPos(movePos, UIAppPanel.UISpeed);
    }

    protected virtual void CloseNow()
    {
        Rtransform.anchoredPosition = new Vector2(GetOutOfBoundsPosition() * -1, 0f) / 2;
    }

    protected virtual void OpenNow()
    {
        Rtransform.anchoredPosition = Vector2.zero;
    }
}
