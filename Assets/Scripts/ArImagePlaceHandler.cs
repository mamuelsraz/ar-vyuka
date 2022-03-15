using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArImagePlaceHandler : MonoBehaviour
{
    ARTrackedImageManager m_ArTrackedImageManager;
    public ObjectLoadHandler load;

    // Start is called before the first frame update
    void Start()
    {
        m_ArTrackedImageManager = GetComponent<ARTrackedImageManager>();
        m_ArTrackedImageManager.trackedImagesChanged += OnImagesChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (AppManager.instance.CurrenState == AppState.ImagePlaceState || AppManager.instance.CurrenState == AppState.LookState)
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
            var item = args.added[0];
            if (AppManager.instance.currentArObjectInstance == null)
                PlaceObject(item.gameObject);
        }
        if (args.updated.Count > 0)
        {
            var item = args.updated[0];
            if (item.trackingState == TrackingState.Tracking && AppManager.instance.currentArObjectInstance == null)
                PlaceObject(item.gameObject);
        }
    }

    void PlaceObject(GameObject anchor)
    {
        AppManager.instance.CreateNewARObjectInstance(AppManager.instance.currentArObject, anchor.transform);
        
        Debug.Log("Placed Object");

        AppManager.instance.CurrenState = AppState.LookState;
    }
}
