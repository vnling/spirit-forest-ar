using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Manager<MenuManager>
{
    public GameObject menu;
    public GameObject menuButton;
    public GameObject placementConfirmation;
    public GameObject animationObject;

    public UIManager uiManager;

    void OnEnable()
    {
        TotemPlacer.onPlacedObject += PlacedObject;
        AltRealityARManager.onExperienceStart += OnExperienceStart;
        AltRealityARManager.onExperienceReset += OnExperienceReset;
    }

    void OnDisable()
    {
        TotemPlacer.onPlacedObject -= PlacedObject;
        AltRealityARManager.onExperienceStart -= OnExperienceStart;
        AltRealityARManager.onExperienceReset -= OnExperienceReset;
    }

    public void OpenMenu()
    {
        menu.SetActive(true);
        menuButton.SetActive(false);
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
        menuButton.SetActive(true);
    }

    void PlacedObject()
    {
        if (!placementConfirmation.activeSelf)
        {
            placementConfirmation.SetActive(true);
        }
    }

    public void OnExperienceStart()
    {
        placementConfirmation.SetActive(false);
        uiManager.Hide();
    }

    public void OnExperienceReset()
    {
        uiManager.Reset();
    }

    public void ResetAR()
    {
        AltRealityARManager.Instance.ResetARTracking();
        CloseMenu();
    }

    public void PauseAR()
    {
        AltRealityARManager.Instance.arSession.enabled = false;
        CloseMenu();
    }

    public void ResumeAR()
    {
        AltRealityARManager.Instance.arSession.enabled = true;
        CloseMenu();
    }
}
