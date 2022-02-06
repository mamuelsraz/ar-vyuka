using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacer : MonoBehaviour
{
    public ARRaycastManager raycastManager;

    public GameObject ObjToPlace;
    public GameObject placementIndicator;

    Pose placementPose = new Pose();
    bool canPlace;

    private void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (canPlace && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Instantiate(ObjToPlace, placementPose.position, placementPose.rotation);
        }
    }

    void UpdatePlacementIndicator()
    {
        if (canPlace)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.AllTypes);
        canPlace = hits.Count > 0;
        if (canPlace)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            Debug.Log(hits[0].hitType);
        }
    }
}
