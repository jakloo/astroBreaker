using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuiController : MonoBehaviour
{
    
    public Image reticleImage;
    public TMP_Text colorText;
    public TMP_Text scoreText;
    private float reticleFullSize;

    public Image[] taskCounters;

    private int shownTaskCounters = 5;

    private Color currentTaskColor;

    void Start()
    {
        reticleFullSize = reticleImage.rectTransform.sizeDelta.x;
    }

    public void updateReticleSize(float percentage){
        float newSize = reticleFullSize - (reticleFullSize * percentage);
        reticleImage.rectTransform.sizeDelta = new Vector2(newSize, newSize);
    }

    public void updateTaskColor(Color color){
        currentTaskColor = color;
    }

    public void updateScore(float score){
        scoreText.text = string.Format("{0:F0}", score);
    }

    public void updateTaskCount(int newCount){
        
        if (newCount < shownTaskCounters){

            for (int i = newCount; i < shownTaskCounters; i++){
                taskCounters[i].enabled = false;
            }

        }

        else if (newCount > shownTaskCounters){

            for (int i = shownTaskCounters; i < newCount; i++){
                taskCounters[i].enabled = true;
                taskCounters[i].color = currentTaskColor;
            }

        }

        shownTaskCounters = newCount;
    }
}
