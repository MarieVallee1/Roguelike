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
        boss.SpawnOursins();
    }

    public void Death()
    {
        boss.StartCoroutine("Death");
    }
}
