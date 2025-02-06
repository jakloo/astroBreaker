using UnityEngine;

public class MenuGazeController : MonoBehaviour
{
    private Transform cameraTransform;
    
    private GameObject currentTarget = null;

    private MenuGuiController gui;

    private MenuLogicController logic;
    private bool highlightingButton = false;

    void Start(){
        cameraTransform = Camera.main.transform;
        gui = GetComponent<MenuGuiController>();
        logic = GetComponent<MenuLogicController>();
    }

    void Update()
    {
        
        RaycastHit hit;
    
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

    private bool raycast(out RaycastHit hit){
        return Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit);
    }

    public GameObject getCurrentTarget(){
        return currentTarget;
    }
}
