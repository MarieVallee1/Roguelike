using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Enemies;
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
    private CharacterData characterData;
    public int idleBetweenAttacks = 1;
    private int idleBetweenAttacksCount = 0;
    [SerializeField] private Oursin usedOursin;

    // Caché //
    public float hiddenDistance = 1;
    private bool enableAttack;
    private bool hidden = true;
    private EnemyHealth enemyHealth;

    // Animator //
    public Animator animator;
    private float randomStartTiming;
    private void Start()
    {
        target = PlayerController.Instance.gameObject.transform.GetChild(6);
        characterData = PlayerController.Instance.characterData;
        radius = usedOursin.radius;
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnEnable()
    {
        randomStartTiming = Random.Range(0f, 3f);
        StartCoroutine(RandomStart());
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, target.transform.position) <= hiddenDistance)
        {
            animator.SetBool("Hidden", true);
            hidden = true;
            enemyHealth.vulnerable = false;
        }
        else
        {
            animator.SetBool("Hidden", false);
            hidden = false;
            enemyHealth.vulnerable = true;
        }
    }

    public void CreateSpawnList()
    {
        spawnPointList.Clear();
        spawnPointList.Add(target.transform.position);
        for (int i = 0; i < nbOursinAround ; i++)
        {
            spawnPointList.Add( new Vector2(spawnPointList[0].x + Mathf.Cos(2*Mathf.PI/nbOursinAround * i) * radiusSpawn , spawnPointList[0].y + Mathf.Sin(2*Mathf.PI/nbOursinAround * i) * radiusSpawn));
        }

        for (int i = 0; i < spawnPointList.Count; i++)
        {
            if (Physics2D.CircleCast(spawnPointList[i], radius, Vector2.zero, 0,
                    LayerMask.GetMask("Obstacle", "Canonnier", "Mur")))
            {
                spawnPointList.Remove(spawnPointList[i]);
                i -= 1;
            }
        }
    }
    
    public void SpawnOursins()
    {
        if (spawnPointList.Count >= 1)
        {
            int x = Random.Range(0, spawnPointList.Count);
            usedOursin.CannonierShooting(spawnPointList[x]);
            spawnPointList.Remove(spawnPointList[x]);
            idleBetweenAttacksCount = 0;
        }
    }
    public void NumberOfIdleBetweenAttacks()
    {
        idleBetweenAttacksCount += 1;
        if (idleBetweenAttacksCount >= idleBetweenAttacks)
        {
            animator.SetTrigger("Shoot");
        }
    }

    private IEnumerator RandomStart()
    {
        yield return new WaitForSeconds(randomStartTiming);
        animator.SetTrigger("Shoot");
    }

    public void StartCoroutineTarget(Transform baitTransform)
    {
        StartCoroutine(ChangeTarget(baitTransform));
    }

    private IEnumerator ChangeTarget(Transform baitTransform)
    {
        target = baitTransform;
        yield return new WaitForSeconds(characterData.baitDuration);
        target = PlayerController.Instance.transform.GetChild(6);
    }
}

}
