using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TANUKI_BLACKBOARD : MonoBehaviour
{
    public float foodDetectableRadius;
    public float foodReachedRadius = 12;
    public float timeToEat;

   // public float orbitRadius;
    public GameObject orbitTarget;
    



    void Awake()
    {
        orbitTarget = GameObject.Find("Attractor");

    }
}
