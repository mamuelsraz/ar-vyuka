using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TextSpeech;
using UnityEngine.UI;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class ARPlaceHandler : MonoBehaviour
{
    [SerializeField] GameObject placementIndicator;
    [SerializeField] Button placeButton;
    public ArObject specialObjText;

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
#if UNITY_EDITOR
            AppManager.instance.CreateNewARObjectInstance(AppManager.instance.currentArObject, null);

            if (AppManager.instance.currentArObject == specialObjText)
            {
                AppManager.instance.CurrenState = AppState.TextLookState;
            }
            else
                AppManager.instance.CurrenState = AppState.LookState;
#endif

            if (!placementIndicator.activeSelf) placementIndicator.SetActive(true);

            UpdatePlacementPose();
            UpdatePlacementIndicator();

            placeButton.interactable = placementPoseIsValid;
        }
        else
        {
            if (placementIndicator.activeSelf) placementIndicator.SetActive(false);
        }
    }

    public void TryPlace()
    {
        if (placementPoseIsValid) PlaceObject();
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon);

        placementPoseIsValid = s_Hits.Count > 0;

        if (placementPoseIsValid)
        {
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
            if (!placementIndicator.activeSelf) placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            if (placementIndicator.activeSelf) placementIndicator.SetActive(false);
        }
    }

    void PlaceObject()
    {
        var hitPose = s_Hits[0].pose;
        var hitTrackableId = s_Hits[0].trackableId;
        var hitPlane = m_PlaneManager.GetPlane(hitTrackableId);

        var anchor = m_AnchorManager.AttachAnchor(hitPlane, hitPose);

        if (anchor == null)
        {
            Debug.Log("Error creating anchor.");
        }
        else
        {
            m_AnchorPoints.Add(anchor);

            AppManager.instance.CreateNewARObjectInstance(AppManager.instance.currentArObject, anchor.transform);

            if (AppManager.instance.currentArObject == specialObjText) AppManager.instance.CurrenState = AppState.TextLookState;
            else AppManager.instance.CurrenState = AppState.LookState;
        }
    }
}
