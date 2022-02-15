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
            Open();
        }
        else Close();

        AppManager.instance.OnStateEnter.AddListener(TryOpen);
        AppManager.instance.OnStateExit.AddListener(TryClose);
    }

    void TryOpen(AppState state)
    {
        if (state == targetState) Open();
    }

    void TryClose(AppState state)
    {
        if (state == targetState) Close();
    }

    protected virtual void Open()
    {
        Rtransform.DOAnchorPos(Vector2.zero, UIAppObject.UISpeed);
    }

    protected virtual void Close()
    {
        Vector2 movePos = new Vector2(ScreenSize.x * -1f, 0f);
        Rtransform.DOAnchorPos(movePos, UIAppObject.UISpeed);
    }
}
