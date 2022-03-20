using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUIButton : MonoBehaviour
{
    public Button downloadButton;
    public Button playButton;
    public Button QRButton;
    
    [HideInInspector] public ChooseModeQR qrChooseMode;
    [HideInInspector] public ArObject ARObject;

    bool qrSubbed;
    private void Start()
    {
        ObjectLoadHandler.instance.OnLoadedArObj.AddListener(ObjLoaded);
        ObjectLoadHandler.instance.OnErrorArObj.AddListener(ObjError);
        downloadButton.onClick.AddListener(Download);
        playButton.onClick.AddListener(Play);
        QRButton.onClick.AddListener(QRToggle);

        downloadButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        QRButton.gameObject.SetActive(false);
    }

    void ObjLoaded()
    {
        downloadButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        QRButton.gameObject.SetActive(true);
    }

    void ObjError()
    {
        downloadButton.interactable = true;
    }

    public void QRToggle()
    {
        if (qrSubbed)
        {
            qrChooseMode.UnSubscribe(ARObject);
        }
        else
        {
            qrChooseMode.Subscribe(ARObject);
        }

        qrSubbed = !qrSubbed;
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
        AppManager.instance.CreateNewARObjectInstance(AppManager.instance.currentArObject, transform);
        AppManager.instance.DestroyCurrentArObjInstance();

        AppManager.instance.CurrenState = AppState.PlaceState;
    }
}
