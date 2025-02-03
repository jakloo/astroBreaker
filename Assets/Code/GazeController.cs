using UnityEngine;

public class GazeController : MonoBehaviour
{
    private Transform cameraTransform;
    
    private const float MAX_GAZE_TIME = 1f;

    private float gazeTimer = 0f;
    private GameObject currentTarget = null;

    private GuiController gui;

    private LogicController logic;
    private bool highlightingButton = false;

    void Start(){
        cameraTransform = Camera.main.transform;
        gui = GetComponent<GuiController>();
        logic = GetComponent<LogicController>();
    }

    void Update()
    {
        
        RaycastHit hit;
        
        if (logic.menuMode){
    
            if (!raycast(out hit)){
                if (highlightingButton){
                    gui.stopHighlightingButton(currentTarget);
                    highlightingButton = false;
                }
                currentTarget = null;
                return;
            }
                
            GameObject hitObject = hit.collider.gameObject;
            if (!hitObject.CompareTag("Button"))
                return;

            if (hitObject == currentTarget)
                return;

            if (highlightingButton){
                gui.stopHighlightingButton(currentTarget);
                highlightingButton = false;
            }

            gui.highlightButton(hitObject);
            currentTarget = hitObject;
            highlightingButton = true; 

        }

        else if (raycast(out hit)) {

            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != currentTarget) {
                currentTarget = hitObject;
                updateGazeTime(0f);
                return;
            }

            if (hitObject.CompareTag("Floor")){
                gui.showPauseHint();
                logic.canPauseGame = true;
                return;
            }

            if (!logic.canHitTarget(hitObject))
                return;
            
            if (hitObject.CompareTag("Upgrade")){
                UpgradeType type = hitObject.GetComponent<UpgradeBehaviour>().type;
                gui.showUpgradeDescription(type);
            } else {
                gui.hideUpgradeDescription();
            }

            updateGazeTime(gazeTimer + Time.deltaTime);

            if (gazeTimer >= MAX_GAZE_TIME) {
                logic.destroyTarget(hitObject);
                resetGaze();
        
            }
        }

        else resetGaze();
    }

    private bool raycast(out RaycastHit hit){
        return Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit);
    }

    private void updateGazeTime(float updatedValue) {

        gazeTimer = updatedValue;
        float percentage = Mathf.Clamp01(updatedValue / MAX_GAZE_TIME);
        gui.updateReticleSize(percentage);

    }

    private void resetGaze(){
        updateGazeTime(0f);
        gui.hideUpgradeDescription();
        gui.hidePauseText();
        logic.canPauseGame = false;
        if (highlightingButton){
            gui.stopHighlightingButton(currentTarget);
            highlightingButton = false;
        }
        currentTarget = null;
    }

    public void activateMenuMode(){
        resetGaze();
    }

    public void deactivateMenuMode(){
        if (highlightingButton){
                gui.stopHighlightingButton(currentTarget);
                highlightingButton = false;
        }
    }

    public GameObject getCurrentTarget(){
        return currentTarget;
    }
}
