using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableType { Grab, Force }

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    public InteractableType interactableType = InteractableType.Grab;

    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTouchDown(Vector3 startTouchPosition)
    {
        Debug.Log("on touch down");

        if (interactableType == InteractableType.Grab)
        {
            rigidbody.isKinematic = true;
        }
    }

    public void OnTouchUp()
    {
        Debug.Log("on touch up");
        rigidbody.isKinematic = false;
    }

    public void Move(Vector3 currentTouchPosition)
    {
        switch (interactableType)
        {
            case InteractableType.Grab:
                MovePosition(currentTouchPosition);
                break;
        }
    }

    private void MovePosition(Vector3 currentTouchPosition)
    {
        rigidbody.MovePosition(currentTouchPosition);
    }
}
