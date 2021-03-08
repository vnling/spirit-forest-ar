using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AltReality: add editor support
// - to rorate camera with mouse or touchpad while pressing down Left Alt key
// - walk the camera with keypress
public class VrEditorEmulator : MonoBehaviour
{
    public Transform mainCamera;

    [Range(.05f, 2)]
    public float dragRate = .2f;

    [Range(.1f, 2f)]
    public float walkDistance = 1f;

    private Quaternion initialRotation;
    private Quaternion attitude;
    private Vector2 dragDegrees;
    private Vector3 lastMousePos;

    private Vector3 translatePos;

    void Awake()
    {
        initialRotation = mainCamera.rotation;
    }

    void Update()
    {
#if UNITY_EDITOR
        SimulateVR();

        attitude = initialRotation * Quaternion.Euler(dragDegrees.x, 0, 0);
        mainCamera.rotation = Quaternion.Euler(0, -dragDegrees.y, 0) * attitude;

        SimulateWalking();
#endif
    }

    void SimulateVR()
    {
        var mousePos = Input.mousePosition;
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            var delta = mousePos - lastMousePos;
            dragDegrees.x -= delta.y * dragRate;
            dragDegrees.y -= delta.x * dragRate;
        }
        lastMousePos = mousePos;

    }

    void SimulateWalking()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Make it move x meters per second instead of x meters per frame
        x *= Time.deltaTime;
        z *= Time.deltaTime;

        translatePos.Set(x, 0f, z);

        // Rotate the moving direction with camera's rotation
        translatePos = mainCamera.rotation * translatePos;

        // Restrict the y to zero
        translatePos.y = 0f;

        transform.Translate(translatePos);
    }
}
