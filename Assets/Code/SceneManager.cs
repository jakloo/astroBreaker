using UnityEngine.SceneManagement;

public class SceneLoader {

    public static void restartLevel(){
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public static void loadMainMenu(){
        SceneManager.LoadScene(0);
    }

    public static void loadSpace(){
        SceneManager.LoadScene(1);
    }

    public static void loadAtmosphere(){
        SceneManager.LoadScene(2);
    }

}