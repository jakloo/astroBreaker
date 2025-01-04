using System;
using System.Collections;
using UnityEngine;

public class LogicController : MonoBehaviour
{
    
    private float spawnTimeInterval;

    int junkCounter = 0;

    public const int COLOR_COUNT = 6;
    private const int MAX_TASK_COUNT = 5;
    private readonly Color[] colors = new Color[COLOR_COUNT];

    private readonly int[] junkCounterPerColor = new int[COLOR_COUNT];

    private readonly System.Random rng = new();

    const int MAX_JUNK_COUNT_SOFT = 20;
    const int MAX_JUNK_COUNT_HARD = 40;
    const float BASE_JUNK_SPAWN_TIME = 0.1f;
    const float MAX_JUNK_SPAWN_TIME_GAIN = 5.0f;

    private int currentColorIndex;

    private float score;

    private int taskCount = 0;

    private int lifeCount = 3;

    const float MAX_TIME_LIMIT = 10f;

    private float timeElapsed = 0f;

    GuiController gui;
    
    void Start()
    {

        addColorToLists(0, "Red", 145, 0, 0);
        addColorToLists(1, "Green", 25, 115, 25);
        addColorToLists(2, "Blue", 70, 120, 230);
        addColorToLists(3, "Yellow", 225, 225, 0);
        addColorToLists(4, "Cyan", 40, 255, 200);
        addColorToLists(5, "Pink", 200, 0, 160);

        for (int i = 0; i < COLOR_COUNT; i++){
            junkCounterPerColor[i] = 0;
        }

        gui = GetComponent<GuiController>();

    }

    void Update() {
        
        timeElapsed += Time.deltaTime;
        float percentage = timeElapsed / MAX_TIME_LIMIT;
        Debug.Log(percentage);
        gui.updateTimeBarSize(percentage);

    }

    void addColorToLists(int index, string name, float red, float green, float blue){
        colors[index] = new Color(red/255f, green/255f, blue/255f);
    }

    public bool canSpawnJunk(float timeElapsed){
        if (junkCounter > MAX_JUNK_COUNT_HARD)
            return false;
        return timeElapsed >= spawnTimeInterval;
    }

    public void registerNewJunk(GameObject junk){

        int colorIndex;

        if (junkCounterPerColor[currentColorIndex] < 2)
            colorIndex = currentColorIndex;
        else colorIndex = rng.Next(COLOR_COUNT);

        junk.tag = colorIndex.ToString();
        junk.GetComponent<Renderer>().material.color = colors[colorIndex];

        if (junkCounter == 0){
            changeTaskCount(0);
            changeCurrentColor(colorIndex);
            changeTaskCount(1);
        }

        junkCounter++;
        junkCounterPerColor[colorIndex]++;

        spawnTimeInterval =  calculateNextSpawnTime();
    }

        public bool canHitJunk(GameObject junk){
        int colorIndex = int.Parse(junk.tag);
        return colorIndex == currentColorIndex;
    }

    public void destroyJunk(GameObject junk){

        junkCounter--;
        changeTaskCount(taskCount-1);

        int colorIndex = int.Parse(junk.tag);
        junkCounterPerColor[colorIndex]--;

        ValueDescriptor value = junk.GetComponent<ValueDescriptor>();
        float scoreGain = 5 + Mathf.RoundToInt(5 * value.scaleValue) + Mathf.RoundToInt(5 * value.speedValue);
        score += scoreGain;
        gui.updateScore(score);

        Destroy(junk);

        if (taskCount == 0){
            changeCurrentColor(chooseNextColor());
            changeTaskCount(chooseNextTaskCount());
        }

    }

    private void changeCurrentColor(int colorIndex){
        currentColorIndex = colorIndex;
        gui.updateTaskColor(colors[colorIndex]);
    }

    private int chooseNextColor(){

        BitArray restricted = new(COLOR_COUNT);
        restricted[currentColorIndex] = true;

        for (int i = 0; i < COLOR_COUNT; i++){
            int count = junkCounterPerColor[i];
            if (count == 0)
                restricted[i] = true;
            else if (count > junkCounter/2){
                return i;
            }
                
        }

        int[] choosable = new int[COLOR_COUNT];
        int choosableCount = 0;

        for (int i = 0; i < COLOR_COUNT; i++){
            if (restricted[i] == false){
                choosable[choosableCount] = i;
                choosableCount++;
            }
        }

        if (choosableCount == 0){
            return rng.Next(COLOR_COUNT);
        }

        return choosable[rng.Next(choosableCount)];

    }

    private void changeTaskCount(int count){
        taskCount = count;
        gui.updateTaskCount(count);
    }

    private int chooseNextTaskCount(){
        return Math.Min(MAX_TASK_COUNT, junkCounterPerColor[currentColorIndex]);
    }

    private int countJunkOfOtherColors(int indexToIgnore){

        int count = 0;

        for (int i = 0; i < COLOR_COUNT; i++){
            if (i == indexToIgnore)
                continue;
            count += junkCounterPerColor[i];
        }

        return count;
    }

    public float calculateNextSpawnTime(){
        return  BASE_JUNK_SPAWN_TIME + MAX_JUNK_SPAWN_TIME_GAIN -
                Mathf.Max(0, (MAX_JUNK_COUNT_SOFT - junkCounter) / (MAX_JUNK_COUNT_SOFT / MAX_JUNK_SPAWN_TIME_GAIN));
    }
}
