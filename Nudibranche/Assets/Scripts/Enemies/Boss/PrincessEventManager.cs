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

    public void Slash()
    {
        boss.PlaySlashSound();
    }

    public void Dash()
    {
        boss.PlayDashSound();
    }

    public void Vulnerable()
    {
        boss.vulnerable = true;
        Debug.Log("vulnerable");
    }
    
    public void Invulnerable()
    {
        boss.vulnerable = false;
    }
}
