using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using Oursins;

namespace Ennemy
{
    public class Cannonier : MonoBehaviour
{
    //Spawn Oursin//
    public List<Vector2> spawnPointList;
    public float radiusSpawn = 2;
    private float radius = 2;
    public int nbOursinAround = 6;
    public Transform target;
    public int nbOursin = 3;
    public float timeBetweenAttacks = 3;
    [SerializeField] private Oursin usedOursin;

    // Caché //
    public float hiddenDistance = 1;
    private bool enableAttack;
    private bool hidden = true;
    private bool Hidden
    {
        get { return hidden;}
        set
        {
            if (value != hidden)
            {
                if (hidden)
                {
                    StartCoroutine(DelayBetweenAttacks());
                }
                else
                {
                    StopCoroutine(DelayBetweenAttacks());
                }
            }
            hidden = value; 
        }
    }
    
    // Animator //
    private Animator animator;

    private void Start()
    {
        target = PlayerController.instance.transform;
        radius = usedOursin.radius;
        animator = GetComponent<Animator>();
        animator.SetTrigger("Activate");
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, target.transform.position) <= hiddenDistance)
        {
            animator.SetBool("Hidden", true);
            Hidden = true;
        }
        else
        {
            animator.SetBool("Hidden", false);
            Hidden = false;
        }
    }

    void CreateSpawnList()
    {
        spawnPointList.Add(target.transform.position);
        for (int i = 0; i < nbOursinAround ; i++)
        {
            spawnPointList.Add( new Vector2(spawnPointList[0].x + Mathf.Cos(2*Mathf.PI/nbOursinAround * i) * radiusSpawn , spawnPointList[0].y + Mathf.Sin(2*Mathf.PI/nbOursinAround * i) * radiusSpawn));
        }

        for (int i = 0; i < spawnPointList.Count; i++)
        {
            if (Physics2D.CircleCast(spawnPointList[i], radius, Vector2.zero, 0,
                    LayerMask.GetMask("Obstacle", "Canonnier")))
            {
                spawnPointList.Remove(spawnPointList[i]);
                i -= 1;
            }
        }
    }
    
    void SpawnOursins()
    {
        for (int i = 0; i < nbOursin; i++)
        {
            if (spawnPointList.Count != 0)
            {
                int x = Random.Range(0, spawnPointList.Count);
                usedOursin.CannonierShooting(spawnPointList[x]);
                spawnPointList.Remove(spawnPointList[x]);
            }
        }
    }
    public void Attaque()
    {
        CreateSpawnList();
        SpawnOursins();
        spawnPointList.Clear();
    }

    IEnumerator DelayBetweenAttacks()
    {
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(timeBetweenAttacks);
        StartCoroutine(DelayBetweenAttacks());
    }
}

}
