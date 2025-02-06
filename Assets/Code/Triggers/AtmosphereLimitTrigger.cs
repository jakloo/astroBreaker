using UnityEngine;

public class AtmosphereLimitTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        other.GetComponent<ParticleSystem>().Play();
    }
}
