using UnityEngine;

public class MenuLogicController : MonoBehaviour
{

    public GameMode chosenMode;

    public bool menuMode = false;

    public MenuGuiController gui;
    
    void Start()
    {   
        
        gui = GetComponent<MenuGuiController>();
        
    }

    void Update() {

    }

    public void click(){
        GameObject clickedButton = GetComponent<MenuGazeController>().getCurrentTarget();
        if (clickedButton == null || !clickedButton.CompareTag("Button"))
            return;
        handleButtonClick(clickedButton.GetComponent<ButtonBehaviour>().type);
    }

    private void handleButtonClick(ButtonType type){
        switch(type){
            case ButtonType.QUIT_APP:
                Application.Quit();
                break;
            case ButtonType.NEW_GAME:
            case ButtonType.BACK_TO_GAME_MODE:
                gui.switchToScreen(MenuGuiScreen.GAME_MODE);
                break;
            case ButtonType.CHOOSE_SPACE:
                chosenMode = GameMode.SPACE;
                gui.switchToScreen(MenuGuiScreen.COLORS);
                break;
            case ButtonType.CHOOSE_ATMOSPHERE:
                chosenMode = GameMode.ATMOSPHERE;
                gui.switchToScreen(MenuGuiScreen.COLORS);
                break;
            case ButtonType.BACK_TO_MAIN:
                gui.switchToScreen(MenuGuiScreen.MAIN);
                break;
            case ButtonType.CHOOSE_NEON:
                ColorGetter.offset = 0;
                startGame();
                return;
            case ButtonType.CHOOSE_PASTEL:
                ColorGetter.offset = 1;
                startGame();
                break;
        }
    }

    private void startGame(){
        if (chosenMode == GameMode.SPACE)
            SceneLoader.loadSpace();
        else SceneLoader.loadAtmosphere();
    }

}
