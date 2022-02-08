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

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    List<ARAnchor> m_AnchorPoints;

    ARRaycastManager m_RaycastManager;
    ARAnchorManager m_AnchorManager;
    ARPlaneManager m_PlaneManager;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_AnchorPoints = new List<ARAnchor>();

        GameObject h = ObjectToPlace.Instantiate(Vector3.zero);
        Debug.LogError(h.GetComponentInParent<ArObjectHolder>() != null);
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
            return;

        //we hit an object
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("HIT");
        }
        if (hit.collider.gameObject.name == "Cube")
        {
            Debug.LogWarning("Hit something");
            PlaySound(hit);
            return;
        }

        if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            var hitTrackableId = s_Hits[0].trackableId;
            var hitPlane = m_PlaneManager.GetPlane(hitTrackableId);

            var anchor = m_AnchorManager.AttachAnchor(hitPlane, hitPose);

            ObjectToPlace.Instantiate(anchor.transform);

            Debug.Log("Spawning");

            if (anchor == null)
            {
                Debug.Log("Error creating anchor.");
            }
            else
            {
                m_AnchorPoints.Add(anchor);
            }
        }
    }

    void PlaySound(RaycastHit hit)
    {
        GameObject obj = hit.collider.gameObject;

        if (Random.Range(0f, 1f) > 0.5f)
        {
            TextToSpeech TTSManager = TextToSpeech.instance;
            TTSManager.Setting("en-EN", TTSManager.pitch, TTSManager.rate);
            TTSManager.StartSpeak("Cube");
        }
        else
        {
            TextToSpeech TTSManager = TextToSpeech.instance;
            TTSManager.Setting("fr-FR", TTSManager.pitch, TTSManager.rate);
            TTSManager.StartSpeak("Cube");
        }

    }
}
