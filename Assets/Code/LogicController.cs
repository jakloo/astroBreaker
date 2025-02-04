using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicController : MonoBehaviour
{
    
    private float spawnTimeInterval;

    int junkCounter = 0;

    public const int COLOR_COUNT = 6;
    private const int MAX_TASK_COUNT = 5;

    public GameMode stage;

    private readonly int[] junkCounterPerColor = new int[COLOR_COUNT];

    private readonly System.Random rng = new();

    const int MAX_JUNK_COUNT_SOFT = 20;
    const int MAX_JUNK_COUNT_HARD = 40;
    const float BASE_JUNK_SPAWN_TIME = 0.1f;
    const float MAX_JUNK_SPAWN_TIME_GAIN = 9.9f;

    private int currentColorIndex = 0;

    private int taskCount = 0;

    private int lifeCount = 3;

    private float MAX_TIME_LIMIT = 15f;

    private float PHASE_DURATION = 15f;

    private const float FREEZE_DURATION = 10f;

    private float taskTimer = 0f;
    
    private float phaseTimer = 0f;

    private float freezeTimer = 0f;
    private bool shouldSpawnUpgrade = false;

    public bool shouldIncreaseDifficulty = false;

    public bool gameIsOver = false, canPauseGame = false;

    public bool freezeActive = false;

    public bool menuMode = false;

    private int doubleStrikeCharges = 0;

    public static List<GameObject> allTargets = new List<GameObject>();

    public GuiController gui;

    public GameObject projectilePrefab;

    public GameObject projectileSource;

    public float MIN_DISTANCE, MAX_DISTANCE;

    public Material destroyedMaterial;
    
    void Start()
    {   
        
        StatTracker.reset();
        
        for (int i = 0; i < COLOR_COUNT; i++){
            junkCounterPerColor[i] = 0;
        }

        gui = GetComponent<GuiController>();

        if (stage == GameMode.ATMOSPHERE){
            gui.swapTool(currentColorIndex, 0);
            gui.coloriseTools(COLOR_COUNT);
        }
        
    }

    void Update() {

        if (menuMode)
            return;

        float percentage;

        if (freezeActive){
            freezeTimer += Time.deltaTime;
            percentage = freezeTimer / FREEZE_DURATION;
            gui.updateFreezeBarSize(percentage);  
            if (freezeTimer >= FREEZE_DURATION) {
                unfreeze();
            }
            return;
        }

        phaseTimer += Time.deltaTime;
        if (phaseTimer >= PHASE_DURATION){
            shouldSpawnUpgrade = true;
            shouldIncreaseDifficulty = true;
            StatTracker.stats.time += phaseTimer;
            phaseTimer = 0;
        }

        if (stage == GameMode.SPACE){
            taskTimer += Time.deltaTime;
            percentage = taskTimer / MAX_TIME_LIMIT;
            gui.updateTimeBarSize(percentage);        

            if (percentage >= 1){
                resetTimer();
                loseLife();
            }
        }

    }


    private void pauseGame(){
        pauseTime();
        gui.hideGuiScreen(GuiScreen.GAME);
        menuMode = true;
        GetComponent<GazeController>().activateMenuMode();
    }

    private void pauseTime(){
        if (menuMode || freezeActive)
            return;
        foreach (GameObject target in allTargets){
            if (target == null)
                continue;
            var behaviour = target.GetComponent<TargetBehaviour>();
            if (behaviour.destroyed)
                continue;
            behaviour.freeze();
        }
        //timeRunning = false;
    }

    public void freeze(){

        freezeTimer = 0;
        if (freezeActive)
            return;

        pauseTime();

        freezeActive = true;
        gui.showFreezePanel();
    }

    private void unfreeze(){
        cancelFreeze();
        unpauseTime();
    }

    private void cancelFreeze(){
        freezeActive = false;
        gui.hideFreezePanel();
    }

    private void unpauseGame(){
        menuMode = false;
        unpauseTime();
        gui.showGuiScreen(GuiScreen.GAME);
        gui.hidePauseText();
        GetComponent<GazeController>().deactivateMenuMode();
    }

    private void unpauseTime(){
        if (menuMode || freezeActive)
            return;
        foreach (GameObject target in allTargets){
            if (target == null)
                continue;
            var behaviour = target.GetComponent<TargetBehaviour>();
            if (behaviour.destroyed)
                continue;
            behaviour.unfreeze();
        }
        //timeRunning = true;
    }

    public void loseLife(){
        if (lifeCount == 0)
            return;
        lifeCount -= 1;
        gui.updateLiveCount(lifeCount);
        if (lifeCount == 0)
            endGame();
    }

    private void endGame(){
        gameIsOver = true;
        cancelFreeze();
        pauseGame();
        GameStats stats = StatTracker.stats;
        stats.time += phaseTimer;
        if (stats.score > 0)
            stats.timeBonus = stats.time * 0.25f;
        stats.finalScore = (int)stats.score + (int)stats.timeBonus;

        string scoreKey = stage == GameMode.SPACE ? "ScoreSpace" : "ScoreAtmosphere"; 
    
        int bestScore = PlayerPrefs.GetInt(scoreKey, 0);
        stats.bestScore = bestScore;

        if (stats.finalScore > bestScore)
        {
            PlayerPrefs.SetInt(scoreKey, stats.finalScore);
            PlayerPrefs.Save();
        }

        gui.fillStatsGui();
        gui.showGuiScreen(GuiScreen.END_GAME);
    }

    public SpawnCommand canSpawnJunk(float timeElapsed){
        if (shouldSpawnUpgrade) {
            shouldSpawnUpgrade = false;
            return SpawnCommand.SPAWN_UPGRADE;
        }
        if (junkCounter > MAX_JUNK_COUNT_HARD || (timeElapsed < spawnTimeInterval && junkCounter > 0))
            return SpawnCommand.SPAWN_DENIED;

        return SpawnCommand.SPAWN_REGULAR;
    }

    public void registerNewJunk(GameObject junk){

        allTargets.Add(junk);

        int colorIndex;

        if (stage == GameMode.SPACE){
            if (junkCounterPerColor[currentColorIndex] < 1)
                colorIndex = currentColorIndex;
            else colorIndex = rng.Next(COLOR_COUNT);

            if (junkCounter == 0){
                changeTaskCount(0);
                changeCurrentColor(colorIndex);
                changeTaskCount(1);
            }
            
        }
        else {
            colorIndex = rng.Next(COLOR_COUNT);
        }

        var behaviour = junk.GetComponent<TargetBehaviour>();
        behaviour.logic = this;

        behaviour.setColor(colorIndex);
        if (stage == GameMode.ATMOSPHERE)
            behaviour.setBurningColor(colorIndex);

        junkCounter++;
        junkCounterPerColor[colorIndex]++;

        spawnTimeInterval = calculateNextSpawnTime();
    }

    public void registerNewUpgrade(GameObject upgrade){
        allTargets.Add(upgrade);
        int rnd = rng.Next(3)+1;
        upgrade.GetComponent<UpgradeBehaviour>().type = (UpgradeType) rnd;
        var behaviour = upgrade.GetComponent<TargetBehaviour>();
        behaviour.logic = this;
        behaviour.isUpgrade = true;
        spawnTimeInterval =  calculateNextSpawnTime();
    }

    public bool canHitTarget(GameObject target){

        if (target.CompareTag("Upgrade"))
            return true;

        return matchesColor(target);
    }

    public bool matchesColor(GameObject target){
        int colorIndex = -1;
        try{
            colorIndex = int.Parse(target.tag);
        } catch (FormatException){
            Debug.Log("Couldn't convert tag: " + target.tag);
        }
        
        return colorIndex == currentColorIndex;
    }

    public void destroyTarget(GameObject target){


        TargetBehaviour behaviour = target.GetComponent<TargetBehaviour>();
        if (behaviour.destroyed)
            return;

        allTargets.Remove(target);
        behaviour.destroyed = true;
        List<GameObject> secondaryTargets = new List<GameObject>();

        if (target.CompareTag("Upgrade")){
            if (target.GetComponent<UpgradeBehaviour>().type == UpgradeType.FREEZE)
                freeze();
            else if (target.GetComponent<UpgradeBehaviour>().type == UpgradeType.BOOM)
                secondaryTargets = explode(target);
            else if (target.GetComponent<UpgradeBehaviour>().type == UpgradeType.DOUBLE)
                activateDoubleStrike();
            issueToDestroy(projectileSource, target, secondaryTargets);
            return;
        }

        junkCounter--;
        if (stage == GameMode.SPACE)
            changeTaskCount(taskCount-1);
  
        int colorIndex = int.Parse(target.tag);
        junkCounterPerColor[colorIndex]--;

        
        StatTracker.stats.score += calculateScoreGain(target, behaviour);
        gui.updateScore(StatTracker.stats.score);

        

        if (stage == GameMode.SPACE && taskCount == 0){
            changeCurrentColor(chooseNextTaskColor());
            changeTaskCount(chooseNextTaskCount());
        }

        resetTimer();

        if (doubleStrikeCharges > 0 && junkCounterPerColor[colorIndex] > 0){
            applyDoubleStrike(target, secondaryTargets);
        }

        StatTracker.stats.destroyedDirectly++;
        issueToDestroy(projectileSource, target, secondaryTargets);

    }

    private float calculateScoreGain(GameObject target, TargetBehaviour behaviour){
        float scoreGain = 5 + Mathf.RoundToInt(5 * behaviour.scaleValue);
        scoreGain += 5 * (target.GetComponent<Rigidbody>().velocity.magnitude - Spawner.MIN_SPEED_INIT) / (Spawner.MAX_SPEED_LIMIT - Spawner.MIN_SPEED_INIT);
        float distance = Vector3.Distance(transform.position, target.transform.position);
        scoreGain += 5 * Mathf.Max(distance - MIN_DISTANCE, 0) / (MAX_DISTANCE - MIN_DISTANCE);
        return scoreGain; 
    }

    private void applyDoubleStrike(GameObject target, List<GameObject> secondaryTargets){
        GameObject[] junkSharingColor = GameObject.FindGameObjectsWithTag(target.tag);
            float smallestDistance = float.MaxValue;
            GameObject closestTarget = null;
            foreach (GameObject junk in junkSharingColor){
                if (junk == target)
                    continue;
                float distance = (target.transform.position - junk.transform.position).sqrMagnitude;
                if (distance < smallestDistance){
                    smallestDistance = distance;
                    closestTarget = junk;
                }
            }

            if (closestTarget != null){
                secondaryTargets.Add(closestTarget);
                doubleStrikeCharges--;
                gui.updateDoubleStrikeCount(doubleStrikeCharges);
            }
    }

    public void destroyTargetUndirectly(GameObject source, GameObject target){

        if (source == null || target == null)
            return;
        
        TargetBehaviour behaviour = target.GetComponent<TargetBehaviour>();
        List<GameObject> secondaryTargets = new List<GameObject>();
        
        if (behaviour == null || behaviour.destroyed)
            return;

        if (target.CompareTag("Upgrade")){
            if (target.GetComponent<UpgradeBehaviour>().type == UpgradeType.FREEZE)
                freeze();
            else if (target.GetComponent<UpgradeBehaviour>().type == UpgradeType.BOOM)
                secondaryTargets = explode(target);
            else if (target.GetComponent<UpgradeBehaviour>().type == UpgradeType.DOUBLE)
                activateDoubleStrike();
            issueToDestroy(source, target, secondaryTargets);
            allTargets.Remove(target);
            return;
        }

        int colorIndex;

        try{
            colorIndex = int.Parse(target.tag);
            junkCounterPerColor[colorIndex]--;
        } catch (FormatException){
            return;
        }

        if (stage == GameMode.SPACE && matchesColor(target)){
            changeTaskCount(taskCount-1);
            if (taskCount == 0) {
                changeCurrentColor(chooseNextTaskColor());
                changeTaskCount(chooseNextTaskCount());
            }
            resetTimer();
        }

        StatTracker.stats.score += 5;
        gui.updateScore(StatTracker.stats.score);

        allTargets.Remove(target);
        behaviour.destroyed = true;
        StatTracker.stats.destroyedUndirectly++;
        issueToDestroy(source, target, secondaryTargets);

        junkCounter--;
        
    }

    private void activateDoubleStrike(){
        doubleStrikeCharges = 3;
        gui.updateDoubleStrikeCount(doubleStrikeCharges);
    }

    private List<GameObject> explode(GameObject target){

        Collider[] targetsHit = Physics.OverlapSphere(target.transform.position, 10);
        List<GameObject> validTargets = new List<GameObject>();

        foreach (Collider collider in targetsHit){
            
            GameObject targetHit = collider.gameObject;
            if (targetHit == target)
                continue;
            validTargets.Add(targetHit);

        }

        return validTargets;

    }

    private void resetTimer(){
        taskTimer = Mathf.Max(0, taskTimer - (MAX_TIME_LIMIT  * (0.4f + 0.1f * lifeCount)));
        MAX_TIME_LIMIT *= 0.99f;
    }

    public void changeCurrentColor(int colorIndex){
        currentColorIndex = colorIndex;
        gui.updateTaskColor(colorIndex);
    }

    public float calculateNextSpawnTime(){
        return  BASE_JUNK_SPAWN_TIME + MAX_JUNK_SPAWN_TIME_GAIN -
                Mathf.Max(0, (MAX_JUNK_COUNT_SOFT - junkCounter) / (MAX_JUNK_COUNT_SOFT / MAX_JUNK_SPAWN_TIME_GAIN));
    }

    private void issueToDestroy(GameObject source, GameObject target, List<GameObject> secondaryTargets){
        target.layer = LayerMask.NameToLayer("Ignore Raycast");
        Rigidbody body = target.GetComponent<Rigidbody>(); 
        target.GetComponent<Renderer>().material = destroyedMaterial;
        body.isKinematic = true;
        Vector3 direction = (target.transform.position - source.transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, source.transform.position, Quaternion.LookRotation(direction));

        ProjectileBehaviour behavior = projectile.GetComponent<ProjectileBehaviour>(); 
        behavior.setTarget(target);
        behavior.secondaryTargets = secondaryTargets;
        behavior.logic = this;
    }

    public void click(){
        if (canPauseGame){
            pauseGame();
            gui.showGuiScreen(GuiScreen.PAUSE);
            gui.showPauseActiveText();
            canPauseGame = false;
            return;
        }
        if (!menuMode)
            return;
        GameObject clickedButton = GetComponent<GazeController>().getCurrentTarget();
        if (clickedButton == null || !clickedButton.CompareTag("Button"))
            return;
        handleButtonClick(clickedButton.GetComponent<ButtonBehaviour>().type);
    }

    private void handleButtonClick(ButtonType type){
        switch(type){
            case ButtonType.RESTART:
                SceneLoader.restartLevel();
                break;
            case ButtonType.UNPAUSE:
                unpauseGame();
                gui.hideGuiScreen(GuiScreen.PAUSE);
                break;
            case ButtonType.END_GAME:
                unpauseGame();
                gui.hideGuiScreen(GuiScreen.PAUSE);
                endGame();
                break;
            case ButtonType.LEAVE_TO_MENU:
                SceneLoader.loadMainMenu();
                break;
        }
    }

    
    ////// SPACE ONLY
    
       private int chooseNextTaskColor(){

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


    ////// ATNOSPHERE ONLY

    public void chooseNextColor(){
        int nextColorIndex = (currentColorIndex + 1) % COLOR_COUNT;
        gui.swapTool(currentColorIndex, nextColorIndex);
        currentColorIndex = nextColorIndex;
    }

}
