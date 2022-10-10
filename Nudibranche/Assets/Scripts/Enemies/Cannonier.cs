using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Cannonier : MonoBehaviour
{
    //Spawn Oursin//
    public List<Vector2> spawnPointList;
    public float radius = 2;
    public int nbOursin;
    public GameObject target;
    public GameObject oursin;

    private void Start()
    {
        CreateSpawnList();
        for (int i = 0; i < spawnPointList.Count; i++)
        {
            Instantiate(oursin, spawnPointList[i], Quaternion.identity);
        }
    }

    void Update()
    {

        // AstarPath.active.Scan();
        
    }

    void CreateSpawnList()
    {
        spawnPointList.Add(target.transform.position);
        for (int i = 0; i < nbOursin ; i++)
        {
            spawnPointList.Add( new Vector2(spawnPointList[0].x + Mathf.Cos(2*Mathf.PI/nbOursin * i) * radius , spawnPointList[0].y + Mathf.Sin(2*Mathf.PI/nbOursin * i) * radius));
        }

        for (int i = 0; i < spawnPointList.Count; i++)
        {
            if (Physics2D.OverlapCircle(spawnPointList[i], radius, LayerMask.GetMask("Obstacle"), 0, 0))
            {
                spawnPointList.Remove(spawnPointList[i]);
                i -= 1;
            }
        }
    }
}
