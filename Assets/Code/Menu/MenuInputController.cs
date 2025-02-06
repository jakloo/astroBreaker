using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputController : MonoBehaviour {
    

    private MenuLogicController logic;

    InputActions input;

    void Awake(){
        input = new InputActions();
    }

    void Start(){
        logic = GetComponent<MenuLogicController>();
    }

    private void OnEnable()
    {
        input.Gameplay.Enable();
        input.Gameplay.Click.performed += click;
    }

    private void OnDisable()
    {
        input.Gameplay.Click.performed -= click;
        input.Gameplay.Disable();
    }

    private void click(InputAction.CallbackContext context){
        logic.click();
    }

}