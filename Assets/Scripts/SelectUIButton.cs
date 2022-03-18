using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUIButton : MonoBehaviour
{
    public ArObject ARObject;
    public UIObjectPopulator ui;
    public Category category;

    bool clickedQR;

    private void Start()
    {
        ObjectLoadHandler.instance.OnLoadedArObj.AddListener(ObjLoaded);
    }

    void ObjLoaded()
    {
        AppManager.instance.CreateNewARObjectInstance(AppManager.instance.currentArObject, transform);
        AppManager.instance.DestroyCurrentArObjInstance();

        if (clickedQR)
        {
            AppManager.instance.CurrenState = AppState.ImagePlaceState;
        }
        else
        {
            AppManager.instance.CurrenState = AppState.PlaceState;
        }
    }

    public void ClickArObj()
    {
        clickedQR = false;
        AppManager.instance.currentArObject = ARObject;
        ObjectLoadHandler.instance.LoadArObjFromUrl(ARObject);
        //ObjectLoadHandler.instance.OnLoadedArObj.Invoke();
    }

    public void ClickQRObj()
    {
        clickedQR = true;
        AppManager.instance.currentArObject = ARObject;
        //ObjectLoadHandler.instance.OnLoadedArObj.Invoke();
        ObjectLoadHandler.instance.LoadArObjFromUrl(ARObject);
    }
}
