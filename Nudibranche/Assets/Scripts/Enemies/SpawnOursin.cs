using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Pathfinding;

public class SpawnOursin : MonoBehaviour
{
    private float radius;
    private void Start()
    {
        radius = transform.lossyScale.x / 2;
    }

    public void Oursin()
    {

        if (Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Obstacle", "Enemy"), 0, 0))
        {
            Oursin();
        }
    }
    
}
