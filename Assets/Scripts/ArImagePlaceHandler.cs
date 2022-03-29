using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArImagePlaceHandler : MonoBehaviour
{
    public UIImageObjectPopulator SelectedHandler;
    public UIAppPanel uiPanel;

    ARTrackedImageManager m_ArTrackedImageManager;

    // Start is called before the first frame update
    void Start()
    {
        AppManager.instance.OnStateExit.AddListener(ExitMode);
        m_ArTrackedImageManager = GetComponent<ARTrackedImageManager>();
        m_ArTrackedImageManager.trackedImagesChanged += OnImagesChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (AppManager.instance.CurrenState == AppState.ImagePlaceState)
        {
            if (!m_ArTrackedImageManager.enabled)
            {
                m_ArTrackedImageManager.enabled = true;
            }
        }
        else
        {
            if (m_ArTrackedImageManager.enabled) m_ArTrackedImageManager.enabled = false;
        }
    }

    void OnImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        if (args.added.Count > 0)
        {
            foreach (var item in args.added)
            {
                Place(Convert.ToInt32(item.referenceImage.name), item.gameObject);
            }
        }
    }

    void Place(int i, GameObject anchor)
    {
        if (i > SelectedHandler.selected.Count) return;
        AppManager.instance.CreateNewARObjectInstanceNonDestroy(SelectedHandler.selected[i-1], anchor.transform);
    }

    void ExitMode(AppState current, AppState last)
    {
        if (current == uiPanel.targetState)
        {
            Debug.LogWarning("exiting!");
            //DestroyInstances();
        }
    }

    void DestroyInstances()
    {
        foreach (var item in AppManager.instance.CachedInstances)
        {
            AppManager.instance.OnDeletedArObjInstance?.Invoke();

            item.instance.SetActive(false);
        }
    }
}
