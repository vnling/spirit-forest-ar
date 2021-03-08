using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARRaycastManager))]
public class TotemPlacer : MonoBehaviour
{
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    public GameObject totemPrefab;

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject totem { get; private set; }

    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;

    private ARRaycastManager m_RaycastManager;
    private ARSessionOrigin m_ARSessionOrigin;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_ARSessionOrigin = GetComponent<ARSessionOrigin>();
    }

    private void OnDisable()
    {
        if (totem)
            totem.SetActive(false);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if it's interacting with UI
            if (IsPointerOverUIObject(touch.position))
            {
                return;
            }

            if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                Pose hitPose = s_Hits[0].pose;

                if (totem == null)
                {
                    totem = Instantiate(totemPrefab);
                }

                if (!totem.activeSelf) totem.SetActive(true);

                // This does not move the content; instead, it moves the ARSessionOrigin
                // such that the content appears to be at the raycast hit position.
                m_ARSessionOrigin.MakeContentAppearAt(totem.transform, hitPose.position);

                onPlacedObject?.Invoke();
            }
        }
    }

    // source: https://forum.unity.com/threads/ar-foundation-never-blocking-raycaster-on-ui.986688/
    public bool IsPointerOverUIObject(Vector2 touchPosition)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = touchPosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        Debug.Log(raycastResults);
        return raycastResults.Count > 0;
    }
}
