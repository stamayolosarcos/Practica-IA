using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerings
{
    public class Orbit_SB : SteeringBehaviour
    {
        public RotationalPolicy rotationalPolicy = RotationalPolicy.FT;

        private float angularSpeed;

        public GameObject target;

        public override SteeringOutput GetSteering()
        {
            if (this.ownKS == null) this.ownKS = GetComponent<KinematicState>();

            SteeringOutput result = Orbit_SB.GetSteering(this.ownKS, target, angularSpeed);

            base.applyRotationalPolicy(rotationalPolicy, result, target);
            return result;
        }

        public static SteeringOutput GetSteering(KinematicState ownKms, GameObject target, float angularVelocity)
        {
            SteeringOutput steering = new SteeringOutput();

            Vector3 desiredPosition;
            Vector3 desiredVlocity;

            //Moves object around the target
            ownKms.transform.RotateAround(target.transform.position, target.transform.forward, angularVelocity * Time.deltaTime);
            
            //Calculates direction 
            desiredPosition = ownKms.transform.position;
            desiredVlocity = desiredPosition / Time.deltaTime;

            //Applies direction update to the orbitating object
            steering.angularAcceleration = Utils.VectorToOrientation(desiredVlocity / Time.deltaTime);

            steering.angularActive = true;
            steering.linearActive = false;

            return steering;
        }



        public void SetAngularSpeed(float newSpeed)
        {
            angularSpeed = newSpeed;
        }

    }
}