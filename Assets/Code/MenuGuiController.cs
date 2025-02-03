using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuGuiController : MonoBehaviour
{
    
    public Image reticleImage;
    private float reticleFullSize;

    private Image[][] colorImages;

    public Image[] pastelImages;

    public Image[] neonImages;

    public GameObject[] guiScreens;
    public MenuGuiScreen currentScreen;

    public TMP_Text trivia;

    public string[] triviaTexts;

    void Start()
    {
        reticleFullSize = reticleImage.rectTransform.sizeDelta.x;
        colorImages = new Image[][]{neonImages,pastelImages};
        coloriseImages();
    }

    public void updateReticleSize(float percentage){
        float newSize = reticleFullSize - (reticleFullSize * percentage);
        reticleImage.rectTransform.sizeDelta = new Vector2(newSize, newSize);
    }

    public void coloriseImages(){
        for (int i = 0; i<2; i++){
            for (int j = 0; j<6; j++){
                coloriseImage(i,j, ColorGetter.getGuiColors(j,i));
            }
        }    
    }

    public void coloriseImage(int paletteIndex, int colorIndex, Color color){
        colorImages[paletteIndex][colorIndex].color = color;
    }

    public void switchToScreen(MenuGuiScreen screen){
        guiScreens[(int) currentScreen].SetActive(false);
        guiScreens[(int) screen].SetActive(true);
        currentScreen = screen;
        trivia.text = triviaTexts[Random.Range(0,triviaTexts.Length)];
    }

    public void showGuiScreen(GuiScreen screen){
        guiScreens[(int) screen].SetActive(true);
    }

    public void highlightButton(GameObject button){
        Image image = button.GetComponent<Image>();
        image.color = ColorGetter.getHighlightedButtonColor();
    }

    public void stopHighlightingButton(GameObject button){
        Image image = button.GetComponent<Image>();
        image.color = ColorGetter.getDefaultButtonColor();
    }

}
