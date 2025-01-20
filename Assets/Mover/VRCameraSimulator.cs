
using UnityEngine;
using UnityEngine.InputSystem;

public class VRCameraSimulator : MonoBehaviour
{

    private float rotationX = 0;
    private float rotationY = 0;

    private float rotationSpeed = 15;

    private Vector2 lookInput; // Store input from the "Look" action

    InputActions input;

    void Awake(){
        input = new InputActions();
    }

    private void OnEnable()
    {
        // Enable the action map and bind the Look action
        input.Camera.Enable();
        input.Camera.Look.performed += OnLookPerformed;
        input.Camera.Look.canceled += OnLookCanceled;
        Debug.Log("Input enabled!");
    }

    private void OnDisable()
    {
        // Disable the action map and unbind the Look action
        input.Camera.Look.performed -= OnLookPerformed;
        input.Camera.Look.canceled -= OnLookCanceled;
        input.Camera.Disable();
        Debug.Log("Input disabled!");
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        // Read the input from the Look action
        lookInput = context.ReadValue<Vector2>();
        Debug.Log("Look Input: " + lookInput);
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        // Reset look input when the action is canceled
        lookInput = Vector2.zero;
    }

    private void RotateCamera()
    {
        
        #if UNITY_EDITOR
        
        // Adjust input by sensitivity and deltaTime
        rotationX = lookInput.x * rotationSpeed * Time.deltaTime;
        rotationY = lookInput.y * rotationSpeed * -Time.deltaTime;

        // Rotate vertically and clamp to prevent flipping
        // rotationX -= mouseY;
        // rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Apply rotation to the camera (vertical) and player body (horizontal)
        // transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        // transform.Rotate(Vector3.up * mouseX);

        transform.Rotate(0, rotationX, 0);
        Camera.main.gameObject.transform.Rotate(rotationY, 0, 0);

        #endif
    }

    void LateUpdate()
    {

        RotateCamera();
        return;

#if UNITY_EDITOR

        rotationX = /*Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;*/ Mouse.current.position.x.ReadValue();
        rotationY = /*Input.GetAxis("Mouse Y") * Time.deltaTime * -rotationSpeed;*/ Mouse.current.position.y.ReadValue();

        rotationX = (rotationX - 700) / 400;

        transform.Rotate(0, rotationX, 0);
        
        Camera.main.gameObject.transform.Rotate(rotationY, 0, 0);

#endif

    }
}
