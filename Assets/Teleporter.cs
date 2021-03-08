using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform destination;
    private bool beTeleported;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (beTeleported) return;

            Teleporter otherTeleporter = destination.GetComponent<Teleporter>();
            if (otherTeleporter) otherTeleporter.OnTeleport();

            NavigationManager.Instance.transform.position = destination.position - Camera.main.transform.localPosition;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            beTeleported = false;
        }
    }

    public void OnTeleport()
    {
        beTeleported = true;
    }
}
