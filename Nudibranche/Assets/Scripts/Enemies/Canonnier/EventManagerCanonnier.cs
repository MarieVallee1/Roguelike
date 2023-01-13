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

    public void FirstShoot()
    {
        canonnier.SpawnFirstOursin();
    }

    public void Shoot()
    {
        canonnier.SpawnOursins();
    }

    public void IdleBetweenAttacks()
    {
        canonnier.NumberOfIdleBetweenAttacks();
    }

    public void Hidden()
    {
        canonnier.enemyHealth.vulnerable = false;
    }

    public void NotHidden()
    {
        canonnier.enemyHealth.vulnerable = true;
    }
    
    public void ExplosionVFX()
    {
        canonnier.shootVFX.Play();
    }
}
