using System.Collections;
using System.Collections.Generic;
using Ennemy;
using UnityEngine;

public class EventManagerCanonnier : MonoBehaviour
{
    [SerializeField] private Cannonier canonnier;

    public void CreateSpawnList()
    {
        canonnier.CreateSpawnList();
    }

    public void Shoot()
    {
        canonnier.SpawnOursins();
    }

    public void IdleBetweenAttacks()
    {
        canonnier.NumberOfIdleBetweenAttacks();
    }
}
