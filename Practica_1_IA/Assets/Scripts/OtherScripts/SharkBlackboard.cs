using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkBlackboard : MonoBehaviour
{
    public float foodDectableRadius = 60f;
    public float foodReachedRadius = 12f;
    public float timeToEatFood = 2f;

    public GameObject attractor;

    public void Awake()
    {
        attractor = GameObject.Find("FishAttractor");
    }
}
