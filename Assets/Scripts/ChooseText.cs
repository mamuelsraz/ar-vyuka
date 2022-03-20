using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseText : MonoBehaviour
{
    public ArObject textObj;

    public void Play()
    {
        AppManager.instance.currentArObject = textObj;
        AppManager.instance.CreateNewARObjectInstance(AppManager.instance.currentArObject, transform);
        AppManager.instance.DestroyCurrentArObjInstance();

        AppManager.instance.CurrenState = AppState.PlaceState;
    }
}

