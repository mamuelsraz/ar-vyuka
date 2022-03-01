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
        AppManager.currentArObject = ARObject;
    }
}
