using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapCameraController : MonoBehaviour
{
    [Header("Movement Speed")]
    public float moveSpeed = 20f;
    public float zoomSpeed = 10f;
    
    [Header("Map Boundaries")]
    public Vector2 minBounds = new Vector2(0f, 0f);
    public Vector2 maxBounds = new Vector2(100f, 100f);

    [Header("Zoom Constraints")] 
    public float minZoom = 5f;
    public float maxZoom = 40f;
    
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleAllMovement();
    }

    private void HandleAllMovement()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        if (Keyboard.current == null) return;

        Vector2 inputDir = Vector2.zero;
        
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) inputDir.y += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) inputDir.y -= 1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) inputDir.x -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) inputDir.x += 1f;

        inputDir.Normalize();

        Vector3 move = new Vector3(inputDir.x, 0f, inputDir.y);
        transform.position += move * moveSpeed * Time.deltaTime;
        
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        float clampedZ = Mathf.Clamp(transform.position.z, minBounds.y, maxBounds.y);
        
        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
    }

    private void HandleZoom()
    {
       if (Mouse.current == null) return;

       float scroll = Mouse.current.scroll.ReadValue().y;
       
       if (Mathf.Abs(scroll) < 0.01f)
       {
           return;
       }
       
       float newY = transform.position.y - (scroll * zoomSpeed);
       newY = Mathf.Clamp(newY, minZoom, maxZoom);
       
       transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
