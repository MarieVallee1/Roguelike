using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Cannonier : MonoBehaviour
{
    //Oursin//
    private SpawnOursin spawnOursin;

    // Caché//

    private void Start()
    {
        spawnOursin = transform.GetChild(1).GetComponent<SpawnOursin>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            spawnOursin.Oursin();
            AstarPath.active.Scan();
        }
    }
}
