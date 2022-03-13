using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUIButton : MonoBehaviour
{
    public ArObject ARObject;
    public UIObjectPopulator ui;
    public Category category;

    public void ClickArObj()
    {
        AppManager.instance.CurrenState = AppState.PlaceState;
        AppManager.instance.currentArObject = ARObject;
    }

    public void ClickQRObj()
    {
        AppManager.instance.CurrenState = AppState.ImagePlaceState;
        AppManager.instance.currentArObject = ARObject;
    }
}
