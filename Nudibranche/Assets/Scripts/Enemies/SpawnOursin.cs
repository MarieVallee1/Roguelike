using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Pathfinding;

public class SpawnOursin : MonoBehaviour
{
    public RectTransform room;
    private float spawXMax;
    private float spawnXMin;
    private float spawnYMax;
    private float spawnYMin;
    private float radius;
    private void Start()
    {
        radius = transform.lossyScale.x / 2;
        Vector3[] cornerArray = new Vector3[4];
        room.GetWorldCorners(cornerArray);
        spawnXMin = cornerArray[0].x + radius;
        spawXMax = cornerArray[2].x - radius;
        spawnYMin = cornerArray[0].y + radius;
        spawnYMax = cornerArray[2].y - radius;
    }

    public void Oursin()
    {
        transform.position = new Vector2(Random.Range(spawnXMin,spawXMax), Random.Range(spawnYMin,spawnYMax));

        if (Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Obstacle", "Enemy"), 0, 0))
        {
            Oursin();
        }
    }
    
}
