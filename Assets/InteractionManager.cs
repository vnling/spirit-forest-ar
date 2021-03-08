using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    public LayerMask InteractableLayer;

    private Camera mainCamera;
    private Interactable currentInteractable;
    private Vector3 startTouchPosition;
    private float startInteractDistance;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
    DetectFingerInEditor();
#else
    DetectFingerInPhone();
#endif

        MoveInteractable();
    }

    private void MoveInteractable()
    {
        if (currentInteractable)
        {
            Vector3 touchPosition = GetTouchPosition();
            Vector3 currentTouchPositionInWorld = mainCamera.ScreenToWorldPoint(touchPosition);
            currentInteractable.Move(currentTouchPositionInWorld);
        }
    }

    private Vector3 GetTouchPosition()
    {
#if UNITY_EDITOR
    return new Vector3(Input.mousePosition.x, Input.mousePosition.y, startInteractDistance);
#else
    if (Input.touchCount == 0) return Vector3.zero;
    Vector3 touchPosition = Input.GetTouch(0).position;
    return new Vector3(touchPosition.x, touchPosition.y, startInteractDistance);
#endif
    }

    private void DetectFingerInEditor()
    {
        Ray ray;
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50f, InteractableLayer))
            {
                Interactable interactable = hit.rigidbody.GetComponent<Interactable>();

                if (interactable)
                {
                    currentInteractable = interactable;

                    startInteractDistance = hit.distance;
                    startTouchPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, startInteractDistance));
                    currentInteractable.OnTouchDown(startTouchPosition);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (currentInteractable)
            {
                currentInteractable.OnTouchUp();
            }
            currentInteractable = null;
        }
    }

    private void DetectFingerInPhone()
    {
        if (Input.touchCount == 0)
        {
            if (currentInteractable)
            {
                currentInteractable.OnTouchUp();
            }
            currentInteractable = null;
            return;
        }

        Ray ray;
        RaycastHit hit;

        Vector3 touchPosition = Input.GetTouch(0).position;

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ray = mainCamera.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out hit, 50f, InteractableLayer))
            {
                Interactable interactable = hit.rigidbody.GetComponent<Interactable>();

                if (interactable)
                {
                    currentInteractable = interactable;
                    
                    startInteractDistance = hit.distance;
                    startTouchPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, startInteractDistance));
                    currentInteractable.OnTouchDown(startTouchPosition);
                }
            }
        }
    }
}
