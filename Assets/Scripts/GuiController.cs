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
    public Image[] lifeCounters;

    public RawImage timeBar;

    private float timeBarFullSize;
    private float timeBarHeight;

    private int shownTaskCounters = 5;
    private int shownLifeCounters = 3;

    private Color currentTaskColor;

    void Start()
    {
        reticleFullSize = reticleImage.rectTransform.sizeDelta.x;
        timeBarFullSize = timeBar.rectTransform.sizeDelta.x;
        timeBarHeight = timeBar.rectTransform.sizeDelta.y;
    }

    public void updateReticleSize(float percentage){
        float newSize = reticleFullSize - (reticleFullSize * percentage);
        reticleImage.rectTransform.sizeDelta = new Vector2(newSize, newSize);
    }

    public void updateTimeBarSize(float percentage){
        float newSize = timeBarFullSize - (timeBarFullSize * percentage);
        timeBar.rectTransform.sizeDelta = new Vector2(newSize, timeBarHeight);
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

    public void updateLiveCount(int newCount){
        
        if (newCount < shownLifeCounters){

            for (int i = newCount; i < shownLifeCounters; i++){
                lifeCounters[i].enabled = false;
            }
        }

        else if (newCount > shownLifeCounters){

            for (int i = shownLifeCounters; i < newCount; i++){
                lifeCounters[i].enabled = true;
            }

        }

        shownLifeCounters = newCount;
    }
}
