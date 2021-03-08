using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkOnSurface : MonoBehaviour
{
    public LayerMask groundLayer;

    [Range(.1f, 2f)]
    public float fallSpeed = 1f;

    private Transform mainCamera;

    private bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, Vector3.down, out hit, 50f, groundLayer))
        {
            float heightDifference = hit.distance - NavigationManager.Instance.normalHeight;

            if (heightDifference > 0.1f)
            {
                isGrounded = false;
            }
            else if (heightDifference < 0.1f)
            {
                isGrounded = true;
                transform.position = new Vector3(transform.position.x, transform.position.y - heightDifference, transform.position.z);
            }
        }
        else 
        {
            isGrounded = false;
        }

        if (!isGrounded)
        {
            transform.Translate(0, -fallSpeed*Time.deltaTime, 0);
        }
    }
}
