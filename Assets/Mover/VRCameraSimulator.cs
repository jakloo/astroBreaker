
using UnityEngine;

public class VRCameraSimulator : MonoBehaviour
{

    private float rotationX = 0;
    private float rotationY = 0;

    private float rotationSpeed = 200;

    void LateUpdate()
    {

#if UNITY_EDITOR

        rotationX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        rotationY = Input.GetAxis("Mouse Y") * Time.deltaTime * -rotationSpeed;

        transform.Rotate(0, rotationX, 0);
        
        Camera.main.gameObject.transform.Rotate(rotationY, 0, 0);

#endif

    }
}
