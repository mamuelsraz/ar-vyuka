using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUIButton : MonoBehaviour
{
    public Button downloadButton;
    public Button playButton;
    
    [HideInInspector] public ChooseModeQR qrChooseMode;
    [HideInInspector] public ArObject ARObject;
    private void Start()
    {
        ObjectLoadHandler.instance.OnLoadedArObj.AddListener(ObjLoaded);
        ObjectLoadHandler.instance.OnErrorArObj.AddListener(ObjError);
        downloadButton.onClick.AddListener(Download);
        playButton.onClick.AddListener(Play);

        downloadButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
    }

    void ObjLoaded()
    {
        if (AppManager.instance.currentArObject == ARObject)
        {
            downloadButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
        }
    }

    void ObjError()
    {
        downloadButton.interactable = true;
    }

    public void Download()
    {
        AppManager.instance.currentArObject = ARObject;
        downloadButton.interactable = false;
        ObjectLoadHandler.instance.LoadArObjFromUrl(ARObject);
    }

    public void Play()
    {
        AppManager.instance.currentArObject = ARObject;
        //AppManager.instance.CreateNewARObjectInstance(AppManager.instance.currentArObject, transform);
        //AppManager.instance.DestroyCurrentArObjInstance();

        AppManager.instance.CurrenState = AppState.PlaceState;
    }
}
