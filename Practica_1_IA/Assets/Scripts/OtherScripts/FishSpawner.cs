﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

public class FishSpawner : MonoBehaviour
{
    public GameObject sample;

    public int numInstances = 10;
    public float interval = 5f; // one ant every interval seconds
    public float variationRatio = 0.25f;

    private int generated = 0;
    private float elapsedTime = 0f; // time elapsed since last generation


    // Update is called once per frame
    void Update()
    {
        if (generated == numInstances)
            return;

        GameObject clone;
        if (elapsedTime >= interval)
        {
            // spawn creating an instance...
            clone = Instantiate(sample);
            clone.transform.position = this.transform.position;

            KinematicState ks = clone.GetComponent<KinematicState>();
            if (ks != null)
            {
                ks.maxSpeed = ks.maxSpeed + Utils.binomial() * variationRatio * ks.maxSpeed;
                ks.maxAcceleration = ks.maxAcceleration + Utils.binomial() * variationRatio * ks.maxAcceleration;
            }

            FlockingAroundPlusAvoid fk = clone.GetComponent<FlockingAroundPlusAvoid>();
            if (fk != null)
            {
                fk.cohesionThreshold += Utils.binomial() * variationRatio * fk.cohesionThreshold;
                fk.repulsionThreshold += Utils.binomial() * variationRatio * fk.repulsionThreshold;
                fk.wanderRate += Utils.binomial() * variationRatio * fk.wanderRate;

                fk.seekWeight += Utils.binomial() * variationRatio * fk.seekWeight;

                fk.wdWeight += Utils.binomial() * variationRatio * fk.wdWeight;
                fk.rpWeight += Utils.binomial() * variationRatio * fk.rpWeight;
                fk.coWeight += Utils.binomial() * variationRatio * fk.coWeight;
                fk.vmWeight += Utils.binomial() * variationRatio * fk.vmWeight;


            }
            else
            {
                Debug.Log("flocking is null");
            }

            generated++;
            elapsedTime = 0;
        }
        else
        {
            elapsedTime += Time.deltaTime;
        }

    }
}

