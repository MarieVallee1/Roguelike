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
    private void Start()
    {
        Vector3[] cornerArray = new Vector3[4];
        room.GetWorldCorners(cornerArray);
        spawnXMin = cornerArray[0].x + (transform.lossyScale.x / 2);
        spawXMax = cornerArray[2].x - (transform.lossyScale.x / 2);
        spawnYMin = cornerArray[0].y + (transform.lossyScale.x / 2);
        spawnYMax = cornerArray[2].y - (transform.lossyScale.x / 2);
    }

    public void Oursin()
    {
        transform.position = new Vector2(Random.Range(spawnXMin,spawXMax), Random.Range(spawnYMin,spawnYMax));
        
    }
    
    // aller comprendre overlap pour éviter que l'oursin ne chevauche d'autres trucs lordsqu'il spawn
}
