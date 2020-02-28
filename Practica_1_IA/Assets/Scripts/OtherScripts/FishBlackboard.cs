using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBlackboard : MonoBehaviour
{
    public float sharkDetectableRadius = 30f;
    public float sharkFleedRadius = 50f;

    public GameObject attractor;
    public GameObject shark;

    public void Awake()
    {
        shark = GameObject.Find("Shark");
        attractor = GameObject.Find("FishAttractor");
    }
}
