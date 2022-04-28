using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UITtransistor : MonoBehaviour
{
    public float transitionTime = 1;
    public TransitionTransform[] transformsToAnimate;

    private void Start()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            foreach (var t in transformsToAnimate)
            {
                t.MoveToEnd(transitionTime);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            foreach (var t in transformsToAnimate)
            {
                t.MoveToStart(transitionTime);
            }
        }
    }
}

[SerializeField]
public class TransitionTransform 
{
    public RectTransform component;
    public RectTransform stop;

    Vector3 s_position;
    Vector3 s_scale;
    Vector3 s_rotation;

    bool initialized;

    public void Init()
    {
        s_position = component.anchoredPosition;
        s_scale = component.localScale;
        s_rotation = component.eulerAngles;

        initialized = true;
    }

    public void MoveToEnd(float time)
    {
        if (!initialized) Init();

        component.DOAnchorPos(stop.anchoredPosition, time);
        component.DOScale(stop.localScale, time);
        component.DORotate(stop.eulerAngles, time);
    }
    public void MoveToStart(float time)
    {
        component.DOAnchorPos(s_position, time);
        component.DOScale(s_position, time);
        component.DORotate(s_rotation, time);
    }
}
