using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Camera cam;
    private GameObject FOOD;
    
    private GameObject dummy;

    // Start is called before the first frame update
    void Start()
    {
        dummy = new GameObject("dummy");
        cam = Camera.main;
        FOOD = Resources.Load<GameObject>("FOOD");
     
       


        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var position = cam.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            GameObject food = GameObject.Instantiate(FOOD);
            food.transform.position = position;
            food.transform.Rotate(0, 0, Random.value * 360);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var position = cam.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            dummy.transform.position = position;
            GameObject food = SensingUtils.FindInstanceWithinRadius(dummy, "FOOD", 10);
            if (food != null) Destroy(food);
        }

        if (Input.GetMouseButtonDown(2))
        {
            var position = cam.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;

            GameObject Sharpie = GameObject.FindGameObjectWithTag("Sharpie");

            if(Sharpie != null) 
            {
                Sharpie.transform.position = position;
            }

            
                  


            
        }
    }
}

