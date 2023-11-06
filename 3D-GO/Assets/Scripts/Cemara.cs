using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 10f;
    public float turnSpeed = 100f;
    public float zoomSpeed = 10f;

    private Camera freeLookCam;
    private Vector3 dragOrigin;

    void Start() {
        freeLookCam = Camera.main; // You can also assign this through the inspector
    }

    void Update() {
        // WASD Movement
        Vector3 pos = transform.position;

        // Move the camera forward and backward with up and down arrow keys
        if (Input.GetKey(KeyCode.UpArrow))
        {
            pos += transform.forward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            pos -= transform.forward * panSpeed * Time.deltaTime;
        }

        // Move the camera to the left and right with left and right arrow keys
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            pos -= transform.right * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            pos += transform.right * panSpeed * Time.deltaTime;
        }

        // Update the position
        transform.position = pos;
        
        // Zoom with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        freeLookCam.transform.Translate(0, 0, scroll * zoomSpeed, Space.Self);

        // Right-click to rotate camera
        if (Input.GetMouseButtonDown(1)) // Right-click was pressed
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1)) // Right-click is not held down
        {
            return;
        }

        Vector3 pos1 = freeLookCam.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        transform.RotateAround(transform.position, transform.right, -pos1.y * turnSpeed);
        transform.RotateAround(transform.position, Vector3.up, pos1.x * turnSpeed);

        dragOrigin = Input.mousePosition;
    }
}
