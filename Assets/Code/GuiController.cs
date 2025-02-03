using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuiController : MonoBehaviour
{
    
    public Image reticleImage;
    public TMP_Text scoreText;
    public TMP_Text upgradeText;
    private float reticleFullSize;

    public Image[] taskCounters;
    public Image[] lifeCounters;

    public Image[] doubleCounters;

    public GameObject[] guiScreens;

    public RawImage timeBar;
    private float timeBarFullSize;
    private float timeBarHeight;

    public RawImage freezeBar;
    private float freezeBarFullSize;
    private float freezeBarHeight;
    public GameObject freezePanel;

    private int shownTaskCounters = 5;
    private int shownLifeCounters = 3;
    private int shownDoubleCounters = 3;

    private Color currentTaskColor;

    public TMP_Text statValuesText, finalScoreText, bestScoreText, bestScoreInfoText, pauseText;

    private const string PAUSE_HINT_TEXT = "Click to pause the game.";
    private const string PAUSE_ACTIVE_TEXT = "The game is paused.";

    void Start()
    {
        reticleFullSize = reticleImage.rectTransform.sizeDelta.x;
        timeBarFullSize = timeBar.rectTransform.sizeDelta.x;
        timeBarHeight = timeBar.rectTransform.sizeDelta.y;
        freezeBarFullSize = freezeBar.rectTransform.sizeDelta.x;
        freezeBarHeight = freezeBar.rectTransform.sizeDelta.y;
        hideFreezePanel();
        updateDoubleStrikeCount(0);
    }

    public void updateReticleSize(float percentage){
        float newSize = reticleFullSize - (reticleFullSize * percentage);
        reticleImage.rectTransform.sizeDelta = new Vector2(newSize, newSize);
    }

    public void updateTimeBarSize(float percentage){
        float newSize = timeBarFullSize - (timeBarFullSize * percentage);
        timeBar.rectTransform.sizeDelta = new Vector2(newSize, timeBarHeight);
    }

    public void updateFreezeBarSize(float percentage){
        float newSize = freezeBarFullSize - (freezeBarFullSize * percentage);
        freezeBar.rectTransform.sizeDelta = new Vector2(newSize, freezeBarHeight);
    }

    public void updateTaskColor(int colorIndex){
        currentTaskColor = ColorGetter.getGuiColors(colorIndex);
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

    public void updateDoubleStrikeCount(int newCount){

        if (newCount < shownDoubleCounters){

            for (int i = newCount; i < shownDoubleCounters; i++){
                doubleCounters[i].enabled = false;
            }
        }

        else if (newCount > shownDoubleCounters){

            for (int i = shownDoubleCounters; i < newCount; i++){
                doubleCounters[i].enabled = true;
            }

        }

        shownDoubleCounters = newCount;
    }

    public void showUpgradeDescription(UpgradeType type){
        upgradeText.text = type.ToString();
    }

    public void hideUpgradeDescription(){
        upgradeText.text = "";
    }

        public void showFreezePanel(){
        freezePanel.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void hideFreezePanel(){
        freezePanel.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void hideGuiScreen(GuiScreen screen){
        guiScreens[(int) screen].SetActive(false);
    }

    public void showGuiScreen(GuiScreen screen){
        guiScreens[(int) screen].SetActive(true);
    }

    public void fillStatsGui(){
        GameStats stats = StatTracker.stats;
        string timeString = createTimeString(stats.time);
        statValuesText.text =
            $"{stats.destroyedDirectly}\n{stats.destroyedUndirectly}\n{timeString}\n{(int)stats.score}\n{(int)stats.timeBonus}";
        finalScoreText.text = stats.finalScore.ToString();

        if (stats.finalScore > stats.bestScore){
            bestScoreText.text = "New best score!";
            bestScoreInfoText.text = $"Better than the previous by {stats.finalScore - stats.bestScore} points.";
        } else if (stats.finalScore < stats.bestScore){
            bestScoreInfoText.text = $"{stats.bestScore - stats.finalScore} points short of the best score.";
        } else {
            bestScoreInfoText.text = $"Same as the best score! What a coincidence!";
        }

    }

    private string createTimeString(float time){
        int minutes = 0;
        while (time >= 60){
            time -= 60;
            minutes++;
        }
        return $"{minutes}:{(int)time}";
    }

    public void highlightButton(GameObject button){
        Image image = button.GetComponent<Image>();
        image.color = ColorGetter.getHighlightedButtonColor();
    }

    public void stopHighlightingButton(GameObject button){
        Image image = button.GetComponent<Image>();
        image.color = ColorGetter.getDefaultButtonColor();
    }

    public void showPauseHint(){
        pauseText.text = PAUSE_HINT_TEXT;
    }

    public void showPauseActiveText(){
        pauseText.text = PAUSE_ACTIVE_TEXT;
    }

    public void hidePauseText(){
        pauseText.text = "";
    }

    ///////////////// ATMOSPHERE ONLY

    public Image[] toolIcons;

    public void coloriseTools(int colorCount){
        for (int i = 0; i<colorCount; i++){
            coloriseTool(i, ColorGetter.getGuiColors(i));
        }    
    }


    public void coloriseTool(int index, Color color){
        toolIcons[index].color = color;
    }

    public void swapTool(int oldIndex, int newIndex){
        toolIcons[oldIndex].rectTransform.localScale = Vector3.one / 2;
        toolIcons[newIndex].rectTransform.localScale = Vector3.one;
    }

}
