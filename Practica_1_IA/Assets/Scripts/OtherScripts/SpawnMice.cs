using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMice : MonoBehaviour
{
    public GameObject mousePrefab;
    public Transform spawnPos;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) Instantiate(mousePrefab, spawnPos.position, Quaternion.identity);   
    }
}
