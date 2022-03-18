using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIMover : MonoBehaviour
{
    RectTransform rt;
    public Vector2 position;
    Vector2 startPosition;
    bool toggled = false;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        startPosition = rt.anchoredPosition;
    }

    public void MoveTogglePosition()
    {
        if (toggled)
        {
            rt.DOAnchorPos(startPosition, UIAppPanel.UISpeed);
        }
        else 
        {
            rt.DOAnchorPos(position, UIAppPanel.UISpeed);
        }
        toggled = !toggled;
    }

    public void MoveGoalPosition()
    {
        toggled = false;
        MoveTogglePosition();
    }

    public void MoveStartPosition()
    {
        toggled = true;
        MoveTogglePosition();
    }
}
