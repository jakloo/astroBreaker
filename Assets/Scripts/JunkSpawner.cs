using UnityEngine;

public class JunkSpawner : MonoBehaviour {
    
    public GameObject junkPrefab;

    private LogicController logic;

    PositionCalculator positionCalculator = new SpacePositionCalculator();

    //Spawn location fields
    private const float SPAWN_HEIGHT = 8;
    private const float MIN_DISTANCE = 5;
    private const float MAX_DISTANCE = 14;

    //Spawn size fields
    private const float MIN_SCALE = 0.5f;
    private const float MAX_SCALE = 3f;

    //Spawn velocity fields
    private const float MIN_SPEED = 0.5f;
    private const float MAX_SPEED = 2.5f;

    //Spawn time fields
    private float timeCounter = 0;
    
    void Start()
    {
        logic = GetComponent<LogicController>();
        timeCounter = float.MaxValue;
    }

    void Update()
    {
        timeCounter += Time.deltaTime;
        if (logic.canSpawnJunk(timeCounter)){
            Spawn();
            timeCounter = 0;
        }
    }

    private void Spawn(){

        Vector3 spawnPosition = positionCalculator.getRandomSpawnPosition();
        GameObject junk = Instantiate(junkPrefab, spawnPosition, Quaternion.identity);
        logic.registerNewJunk(junk);
        assignScale(junk);
        positionCalculator.assignVelocity(junk);
        
    }

    private void assignScale(GameObject junk){

        float scale = Random.Range(MIN_SCALE, MAX_SCALE);
        junk.transform.localScale = Vector3.one * scale;
        junk.GetComponent<Rigidbody>().mass *= scale;
        junk.GetComponent<ValueDescriptor>().scaleValue = 1 - (scale - MIN_SCALE)/MAX_SCALE;
    
    }

    private abstract class PositionCalculator {

        public abstract Vector3 getRandomSpawnPosition();
        public abstract void assignVelocity(GameObject junk);

    }

    private class AtmospherePositionCalculator : PositionCalculator {

        public override Vector3 getRandomSpawnPosition(){

            float distance = Random.Range(8f, 11f);
            float polarAngle = Random.Range(0, Mathf.PI * 1f);

            return new Vector3(
                distance * Mathf.Cos(polarAngle),
                SPAWN_HEIGHT,
                distance * Mathf.Sin(polarAngle)
            );

        }

        public override void assignVelocity(GameObject junk){
        
            Vector3 direction = new Vector3(0, -1, 0);
            float speed = Random.Range(MIN_SPEED, MAX_SPEED) * 0.25f;
            junk.GetComponent<Rigidbody>().velocity = direction * speed;
            junk.GetComponent<ValueDescriptor>().speedValue = (speed - MIN_SPEED)/MAX_SPEED / ((MAX_SPEED - MIN_SPEED)/MAX_SPEED);

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
            float speed = Random.Range(MIN_SPEED, MAX_SPEED);
            junk.GetComponent<Rigidbody>().velocity = direction * speed;
            junk.GetComponent<ValueDescriptor>().speedValue = (speed - MIN_SPEED)/MAX_SPEED / ((MAX_SPEED - MIN_SPEED)/MAX_SPEED);

        }

    }
}
