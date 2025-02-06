using UnityEngine;

public class TargetBehaviour : MonoBehaviour {
    
        public float scaleValue;
        public Vector3 previousVelocity, previousAngularVelocity;
        public bool destroyed = false;
        bool burning = false;
        public bool isUpgrade = false;

        public LogicController logic;

        Rigidbody body;

        void Start(){
                body = GetComponent<Rigidbody>();
        }

        public void setColor(int index){
                gameObject.tag = index.ToString();
                GetComponent<Renderer>().material.color = ColorGetter.getJunkColor(index);
        }

        public void setBurningColor(int index){
                try{
                        ParticleSystem particles = GetComponent<ParticleSystem>();
                        var colorOverLifetime = particles.colorOverLifetime;
                        Gradient gradient = ColorGetter.getBurningColors(index);
                        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
                } catch (MissingComponentException){}
        }

        public void freeze(){
                
                if (body.isKinematic)
                        return;
                previousVelocity = body.velocity;
                body.velocity = Vector3.zero;
                previousAngularVelocity = body.angularVelocity;
                body.angularVelocity = Vector3.zero;

                try{
                        var particles = GetComponent<ParticleSystem>();
                        if (particles.isPlaying){
                                burning = true;
                                particles.Pause();
                        }

                } catch (MissingComponentException){}
        }

        public void unfreeze(){
                
                body.velocity = previousVelocity;
                body.angularVelocity = previousAngularVelocity;

                try{
                        if (burning)
                                GetComponent<ParticleSystem>().Play();
                } catch (MissingComponentException){}
        }

        private void OnDestroy() {
                if (!(logic.stage == GameMode.ATMOSPHERE) || isUpgrade || destroyed || logic.gameIsOver)
                        return;
                logic.loseLife();
        }


}
