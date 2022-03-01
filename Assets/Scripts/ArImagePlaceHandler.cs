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
                foreach (var item in load.AllObjects[Category.other])
                {
                    if (item.NamesInLanguages[0].name == "Impression, Sunrise by Claude Monet") AppManager.currentArObject = item;
                }
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
            if (AppManager.currentArObjectInstance == null)
                PlaceObject(args.added[0].gameObject);
        }
    }

    void PlaceObject(GameObject anchor)
    {
        GameObject instance = Instantiate(AppManager.currentArObject.obj, anchor.transform);

        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
        AppManager.currentArObjectInstance = new ArObjectInstance(instance, AppManager.currentArObject);

        Debug.Log("Placed Object");

        AppManager.instance.CurrenState = AppState.LookState;
    }
}
