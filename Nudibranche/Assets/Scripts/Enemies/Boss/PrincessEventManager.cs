using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PrincessEventManager : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private VisualEffect tornade;

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

    public void Tornade()
    {
        if (boss.behaviour == Boss.Behaviour.oursins)
        {
            tornade.Play();
        }
    }
}
