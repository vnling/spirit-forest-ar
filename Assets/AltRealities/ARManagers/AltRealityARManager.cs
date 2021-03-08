using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class AltRealityARManager : Manager<AltRealityARManager>
{
    public ARSession arSession;

    public MenuManager menuManager;
    public ARPointCloudManager aRPointCloudManager;
    public ARPlaneManager aRPlaneManager;
    public TotemPlacer totemPlacer;

    public GameObject experienceEnvironment;

    public static event Action onExperienceStart;
    public static event Action onExperienceReset;

    private void Awake()
    {
#if UNITY_EDITOR
        // Hide Animation object
        MenuManager.Instance.animationObject.SetActive(false);
        // Lift camera up
        Camera.main.transform.position = Vector3.up * 1.6f;
#else
        experienceEnvironment.SetActive(false);
#endif
    }

    public void StartExperience()
    {
        // once confirm
        // disable AR Point Cloud Manager, AR Plane Manager, TotemPlacer
        aRPointCloudManager.enabled = false;
        aRPlaneManager.enabled = false;
        totemPlacer.enabled = false;

        foreach (var particle in aRPointCloudManager.trackables)
        {
            particle.gameObject.SetActive(false);
        }

        foreach (var plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

        // enable the environment
        experienceEnvironment.SetActive(true);

        onExperienceStart?.Invoke();
    }

    public void ResetARTracking()
    {
        arSession.Reset();

        // disable the environment
        experienceEnvironment.SetActive(false);

        // bring tracking visuals
        aRPointCloudManager.enabled = true;
        aRPlaneManager.enabled = true;
        totemPlacer.enabled = true;

        foreach (var particle in aRPointCloudManager.trackables)
        {
            particle.gameObject.SetActive(true);
        }

        onExperienceReset?.Invoke();
    }
}
