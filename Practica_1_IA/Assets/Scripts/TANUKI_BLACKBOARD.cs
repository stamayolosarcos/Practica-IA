using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TANUKI_BLACKBOARD : MonoBehaviour
{
    
    public float SharpieDetectableRadius = 500;
    
    public float orbitRadius;
    public float tooFarFromSharpie = 130f;

   // public float orbitRadius;
    public GameObject orbitTarget;
    



    void Awake()
    {
        orbitTarget = GameObject.Find("SHARPIE");
        orbitRadius = 100f;

    }
}
