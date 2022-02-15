using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TextSpeech;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class ARPlacer : MonoBehaviour
{
    [SerializeField] ArObject ObjectToPlace;
    [SerializeField] GameObject placementIndicator;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    List<ARAnchor> m_AnchorPoints;

    ARRaycastManager m_RaycastManager;
    ARAnchorManager m_AnchorManager;
    ARPlaneManager m_PlaneManager;

    private Pose placementPose;
    private bool placementPoseIsValid = false;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_AnchorPoints = new List<ARAnchor>();
    }

    void Update()
    {
        if (AppManager.instance.CurrenState == AppState.PlaceState)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();

            if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceObject();
            }
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon);

        placementPoseIsValid = s_Hits.Count > 0;

        if (placementPoseIsValid)
        {
            Debug.Log($"{placementPose.position}");

            placementPose = s_Hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    void PlaceObject()
    {
        #region help
        /*if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            var hitTrackableId = s_Hits[0].trackableId;
            var hitPlane = m_PlaneManager.GetPlane(hitTrackableId);

            var anchor = m_AnchorManager.AttachAnchor(hitPlane, hitPose);

            ObjectToPlace.Instantiate(anchor.transform);

            if (anchor == null)
            {
                Debug.Log("Error creating anchor.");
            }
            else
            {
                m_AnchorPoints.Add(anchor);
            }
        }*/
        #endregion

        var hitPose = s_Hits[0].pose;
        var hitTrackableId = s_Hits[0].trackableId;
        var hitPlane = m_PlaneManager.GetPlane(hitTrackableId);

        var anchor = m_AnchorManager.AttachAnchor(hitPlane, hitPose);

        ObjectToPlace.Instantiate(anchor.transform);

        if (anchor == null)
        {
            Debug.Log("Error creating anchor.");
        }
        else
        {
            m_AnchorPoints.Add(anchor);
            AppManager.instance.CurrenState = AppState.LookState;
        }
    }
}
