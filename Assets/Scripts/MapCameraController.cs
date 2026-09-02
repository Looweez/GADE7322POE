using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapCameraController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float zoomSpeed = 10f;
    
    public float rotationSpeed = 0.5f;
    
    
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
        HandleRotation(); //i added dis so u can rotate the camera cause its kinda hard to see further away and behind the tower with the depth
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

       // movement relative to where camera is facing
        Vector3 forward = transform.forward;
        forward.y = 0f; // only movew horizontal
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0f;
        right.Normalize();
        
        Vector3 move = (right * inputDir.x) + (forward * inputDir.y);
        
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        if (Mouse.current == null) return;

        
        if (Mouse.current.rightButton.isPressed) //camera rotation with right mouse button (like roblox ykyk)
        {
            
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            transform.Rotate(Vector3.up, mouseDelta.x * rotationSpeed, Space.World);
        }
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
