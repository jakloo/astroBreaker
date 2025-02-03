using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour {
    

    private LogicController logic;

    private bool isHoldingButton = false;

    private const float TIME_NEEDED_TO_TRIGGER = 0.2f;

    private float timeSinceLastTrigger = TIME_NEEDED_TO_TRIGGER;

    InputActions input;

    void Awake(){
        input = new InputActions();
    }

    void Start(){
        logic = GetComponent<LogicController>();
    }

    void Update(){
        timeSinceLastTrigger += Time.deltaTime;
        if (logic.stage == GameMode.SPACE)
            return;
        if (isHoldingButton && timeSinceLastTrigger >= TIME_NEEDED_TO_TRIGGER){
            logic.chooseNextColor();
            timeSinceLastTrigger = 0f;
        }
    }

    private void OnEnable()
    {
        input.Gameplay.Enable();
        input.Gameplay.ChangeColor.performed += startHolding;
        input.Gameplay.ChangeColor.canceled += stopHolding;
        input.Gameplay.Click.performed += click;
    }

    private void OnDisable()
    {
        input.Gameplay.ChangeColor.performed -= startHolding;
        input.Gameplay.ChangeColor.canceled -= stopHolding;
        input.Gameplay.Click.performed -= click;
        input.Gameplay.Disable();
    }

    private void startHolding(InputAction.CallbackContext context){
        isHoldingButton = true;
    }

    private void stopHolding(InputAction.CallbackContext context){
        isHoldingButton = false;
    }

    private void click(InputAction.CallbackContext context){
        logic.click();
    }

}