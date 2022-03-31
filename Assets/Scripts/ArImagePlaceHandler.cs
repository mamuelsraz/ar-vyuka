using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArImagePlaceHandler : MonoBehaviour
{
    public UIImageObjectPopulator SelectedHandler;

    ARTrackedImageManager m_ArTrackedImageManager;
    ARAnchorManager m_AnchorManager;

    List<ArObject> spawned;

    // Start is called before the first frame update
    void Start()
    {
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
                ResetImageTracking();
            }
        }
        else
        {
            if (m_ArTrackedImageManager.enabled)
            {
                m_ArTrackedImageManager.enabled = false;
                DestroyInstances();
            }
        }
    }

    void OnImagesChanged(ARTrackedImagesChangedEventArgs args)
    {

        foreach (var item in args.added)
        {
            Place(Convert.ToInt32(item.referenceImage.name), item.gameObject);

        }
        foreach (var item in args.updated)
        {
            if (item.trackingState == TrackingState.Tracking)
                Place(Convert.ToInt32(item.referenceImage.name), item.gameObject);
        }

    }

    void Place(int i, GameObject anchor)
    {
        if (i > SelectedHandler.selected.Count) return;

        ArObject arObject = SelectedHandler.selected[i - 1];

        if (spawned.Contains(arObject)) return;

        Debug.LogError("spawn " + i);
        AppManager.instance.CreateNewARObjectInstanceNonDestroy(arObject, anchor.transform);
        spawned.Add(arObject);
    }

    void DestroyInstances()
    {
        foreach (var item in AppManager.instance.CachedInstances)
        {
            AppManager.instance.DestroyInstance(item);
        }
    }

    void ResetImageTracking()
    {
        spawned = new List<ArObject>();
    }
}
