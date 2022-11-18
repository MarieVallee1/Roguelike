using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessEventManager : MonoBehaviour
{
    [SerializeField] private Boss boss;

    public void CreateSpawnList()
    {
        boss.CreateSpawnLists();
    }
    
    public void SpawnOursins()
    {
        Debug.Log("called");
        boss.SpawnOursins();
    }
}
