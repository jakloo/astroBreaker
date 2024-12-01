using UnityEngine;

public class JunkDestroyer : MonoBehaviour
{
    private Transform cameraTransform;
    public const float MAX_GAZE_TIME = 1f;
    private float gazeTimer = 0f;
    private GameObject currentTarget = null;

    private GuiController gui;

    private LogicController logic;

    void Start(){
        cameraTransform = Camera.main.transform;
        gui = GetComponent<GuiController>();
        logic = GetComponent<LogicController>();
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit)) {

            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != currentTarget) {
                currentTarget = hitObject;
                updateGazeTime(0f);
                return;
            }

            if (!logic.canHitJunk(hitObject))
                return;

            updateGazeTime(gazeTimer + Time.deltaTime);

            if (gazeTimer >= MAX_GAZE_TIME) {
                logic.destroyJunk(hitObject);
                currentTarget = null;
                updateGazeTime(0f);
        
            }
        }

        else {
            currentTarget = null;
            updateGazeTime(0f);
        }
    }

    private void updateGazeTime(float updatedValue) {

        gazeTimer = updatedValue;
        float percentage = Mathf.Clamp01(updatedValue / MAX_GAZE_TIME);
        gui.updateReticleSize(percentage);

    }
}
