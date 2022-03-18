using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIRotatedButton : MonoBehaviour
{
    public Image image;
    bool toggled;

    public void Toggle()
    {
        if (toggled)
        {
            image.transform.DORotate(new Vector3(0, 0, 270), UIAppPanel.UISpeed);
        }
        else
        {
            image.transform.DORotate(new Vector3(0, 0, 90), UIAppPanel.UISpeed);
        }

        toggled = !toggled;
    }
}
