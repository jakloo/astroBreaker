
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

    private void OnEnable() {
        input.Gameplay.Enable();
        input.Gameplay.Look.performed += OnLookPerformed;
        input.Gameplay.Look.canceled += OnLookCanceled;
    }

    private void OnDisable() {
        input.Gameplay.Look.performed -= OnLookPerformed;
        input.Gameplay.Look.canceled -= OnLookCanceled;
        input.Gameplay.Disable();
    }

    private void OnLookPerformed(InputAction.CallbackContext context) {
        lookInput = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context) {
        lookInput = Vector2.zero;
    }

    private void RotateCamera() {
        rotationX = lookInput.x * rotationSpeed * Time.deltaTime;
        rotationY = lookInput.y * rotationSpeed * -Time.deltaTime;

        transform.Rotate(0, rotationX, 0);
        Camera.main.gameObject.transform.Rotate(rotationY, 0, 0);

    }

    void LateUpdate() {

        RotateCamera();

    }
}
