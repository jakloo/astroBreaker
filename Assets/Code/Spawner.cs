using UnityEngine;

public class Spawner : MonoBehaviour {
    
    public GameObject junkPrefab, upgradePrefab;

    public Mesh[] meshList;

    private LogicController logic;

    PositionCalculator positionCalculator;

    //Spawn location fields
    private const float SPAWN_HEIGHT = 18;
    private const float MIN_DISTANCE = 7.5f;
    private const float MAX_DISTANCE = 12;

    //Spawn size fields
    private const float MIN_SCALE_LIMIT = 0.5f;

    private const float MAX_SCALE_INIT = 4f;

    private const float SCALE_STEP = 0.25f;

    public float SCALE_FACTOR = 0.15f;

    float minScale = MAX_SCALE_INIT - SCALE_STEP*2;
    float maxScale = MAX_SCALE_INIT;

    //Spawn velocity fields
    public const float MAX_SPEED_LIMIT = 7f;

    public const float MIN_SPEED_INIT = 0.25f;

    private const float SPEED_STEP = 0.25f;

    public static float minSpeed = MIN_SPEED_INIT;
    public static float maxSpeed = MIN_SPEED_INIT + SPEED_STEP*2;

    //Spawn time fields
    private float timeCounter = 0;

    private bool increasedMinOnLastDifficultyIncrease = false;
    
    void Start()
    {
        logic = GetComponent<LogicController>();
        timeCounter = float.MaxValue;

        if(logic.stage == GameMode.SPACE)
            positionCalculator = new SpacePositionCalculator();
        else
            positionCalculator = new AtmospherePositionCalculator();
    }

    void Update()
    {
        if (logic.freezeActive || logic.menuMode)
            return;
        
        timeCounter += Time.deltaTime;

        if (logic.shouldIncreaseDifficulty){

            if (increasedMinOnLastDifficultyIncrease){
                if (maxSpeed < MAX_SPEED_LIMIT)
                    maxSpeed += 0.5f;
                if (minScale > MIN_SCALE_LIMIT)
                    maxScale -= 0.5f;
            }
            else {
                if (maxSpeed < MAX_SPEED_LIMIT)
                    minSpeed += 0.5f;
                if (minScale > MIN_SCALE_LIMIT)
                    minScale -= 0.5f;  
            }

            increasedMinOnLastDifficultyIncrease = !increasedMinOnLastDifficultyIncrease;
            logic.shouldIncreaseDifficulty = false;

        }

        SpawnCommand command = logic.canSpawnJunk(timeCounter);

        if (command == SpawnCommand.SPAWN_REGULAR) {
            SpawnJunk();
            timeCounter = 0;
        } else if (command == SpawnCommand.SPAWN_UPGRADE) {
            SpawnUpgrade();
            timeCounter = 0;
        }
        

    }

    private void SpawnJunk(){

        Vector3 spawnPosition = positionCalculator.getRandomSpawnPosition();
        GameObject instance = createInstance(junkPrefab, spawnPosition, true);
        instance.GetComponent<MeshFilter>().mesh = meshList[Random.Range(0, meshList.Length)];
        logic.registerNewJunk(instance);
        assignScale(instance);
        positionCalculator.assignVelocity(instance);
        assignAngularVelocity(instance);
        
    }

    private GameObject createInstance(GameObject prefab, Vector3 position, bool rotate){
        return Instantiate(prefab, position, rotate ? Random.rotation : Quaternion.identity);
    }

    private void SpawnUpgrade(){

        Vector3 spawnPosition = positionCalculator.getRandomSpawnPosition();
        GameObject instance = createInstance(upgradePrefab, spawnPosition, true);
        logic.registerNewUpgrade(instance);
        positionCalculator.assignVelocity(instance);
        assignAngularVelocity(instance);
        
    }

    private void assignScale(GameObject junk){

        float scale = Random.Range(minScale, maxScale) * SCALE_FACTOR;
        junk.transform.localScale = Vector3.one * scale;
        junk.GetComponent<Rigidbody>().mass *= scale;
        junk.GetComponent<TargetBehaviour>().scaleValue = 1 - (scale - MIN_SCALE_LIMIT)/(MAX_SCALE_INIT - MIN_SCALE_LIMIT);
    
    }

    public void assignAngularVelocity(GameObject junk){
        
            Rigidbody body = junk.GetComponent<Rigidbody>();
            body.angularVelocity = body.velocity * 0.25f;

    }

    private abstract class PositionCalculator {

        public abstract Vector3 getRandomSpawnPosition();
        public abstract void assignVelocity(GameObject junk);

    }

    private class AtmospherePositionCalculator : PositionCalculator {

        public override Vector3 getRandomSpawnPosition(){

            float distance = MAX_DISTANCE*1.8f;
            float azimuthAngle = Random.Range(Mathf.PI * -0.3f, Mathf.PI * 0.3f);

            return new Vector3(
                distance * Mathf.Sin(azimuthAngle),
                SPAWN_HEIGHT,
                distance * Mathf.Cos(azimuthAngle)
            );

        }

        public override void assignVelocity(GameObject junk){
        
            Vector3 direction = new Vector3(0, -1, 0);
            float speed = Random.Range(minSpeed, maxSpeed) * 0.35f;
            junk.GetComponent<Rigidbody>().velocity = direction * speed;

        }

    }

    private class SpacePositionCalculator : PositionCalculator {

        public override Vector3 getRandomSpawnPosition(){

            float distance = Random.Range(MIN_DISTANCE, MAX_DISTANCE);
            float polarAngle = Random.Range(Mathf.PI * -0.3f, Mathf.PI * 0.3f);
            float azimuthAngle = Random.Range(Mathf.PI * -0.3f, Mathf.PI * 0.3f);

            return new Vector3(
                distance * Mathf.Sin(azimuthAngle) * Mathf.Cos(polarAngle),
                distance * Mathf.Sin(azimuthAngle) * Mathf.Sin(polarAngle),
                distance * Mathf.Cos(azimuthAngle)
            );

        }

        public override void assignVelocity(GameObject junk){
        
            Vector3 direction = Random.insideUnitSphere.normalized;
            float speed = Random.Range(minSpeed, maxSpeed);
            junk.GetComponent<Rigidbody>().velocity = direction * speed;

        }

    }
}
