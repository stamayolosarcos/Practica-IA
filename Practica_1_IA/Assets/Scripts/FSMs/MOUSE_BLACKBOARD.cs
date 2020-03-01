using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOUSE_BLACKBOARD : MonoBehaviour
{

    public float foodDetectableRadius = 60;
    public float foodReachedRadius = 12;
    public float timeToEat = 1.5f;

    public GameObject attractor;

    
    void Awake()
    {
        attractor = GameObject.Find("LION");
    }

    
}
