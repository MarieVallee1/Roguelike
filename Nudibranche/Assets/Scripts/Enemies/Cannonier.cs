using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using Oursins;

namespace Ennemy
{
    public class Cannonier : MonoBehaviour
{
    // AstarPath.active.Scan();
    
    //Spawn Oursin//
    public List<Vector2> spawnPointList;
    public float radiusSpawn = 2;
    private float radius = 2;
    public int nbOursinAround = 6;
    public GameObject target;
    public int nbOursin = 3;
    public float timeBetweenAttacks = 3;
    [SerializeField] private Oursin usedOursin;

    // Caché //
    private bool hidden;
    public float hiddenDistance = 1;
    private bool enableAttack;
    
    // Animator //
    private Animator animator;

    private void Start()
    {
        radius = usedOursin.radius;
        animator = GetComponent<Animator>();
        animator.SetTrigger("Activate");
        StartCoroutine(DelayBetweenAttacks());
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, target.transform.position) <= hiddenDistance)
        {
            animator.SetBool("Hidden", true);
            hidden = true;
        }
        else
        {
            animator.SetBool("Hidden", false);
            hidden = false;
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
            if (Physics2D.OverlapCircle(spawnPointList[i], radius, LayerMask.GetMask("Obstacle"), 0, 0))
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

    IEnumerator DelayBetweenAttacks()      //un peu crado, fonctionne mais à revoir
    {
        if (!hidden)
        {
            animator.SetTrigger("Shoot");
        }

        yield return new WaitForSeconds(timeBetweenAttacks);
        StartCoroutine(DelayBetweenAttacks());
    }
}

}
