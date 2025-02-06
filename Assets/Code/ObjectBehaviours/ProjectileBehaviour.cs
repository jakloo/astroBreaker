using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    
    public GameObject target;
    public float speed = 10f;
    public List<GameObject> secondaryTargets;

    public Gradient defaultColor;

    public LogicController logic;

    public bool destroyed = false;

    ParticleSystem particles;
    
    // Start is called before the first frame update
    void Start() {
        particles = GetComponent<ParticleSystem>();
    }

    public void setTarget(GameObject target){
        this.target = target;
        Gradient gradient = defaultColor;

        try{
            int colorIndex = int.Parse(target.tag);// + 6 * COLOR_OFFSET;
            gradient = ColorGetter.getProjectileColors(colorIndex);
        } catch (FormatException){}

        particles = GetComponent<ParticleSystem>();
        var colorOverLifetime = particles.colorOverLifetime;
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        
    }

    // Update is called once per frame
    void Update() {

        if (destroyed){
            if (particles.isStopped)
                Destroy(gameObject);
            return;
        }

        if (target == null)
            return;
        
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f) {

            foreach(GameObject secondaryTarget in secondaryTargets){
                if (secondaryTarget == null)
                    continue;
                logic.destroyTargetUndirectly(target, secondaryTarget);
            }

            Destroy(target);
            if (particles.isStopped){
                Destroy(gameObject);
                return;
            }
            destroyed = true;
            
        }

    }

}
