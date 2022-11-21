using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using Oursins;

public class Boss : MonoBehaviour
{
    [Header("Comportement de base")] 
    public Transform target;
    [SerializeField] private float speed = 200;
    private Rigidbody2D rb;

    [Header("Ruée")]
    [SerializeField] private float dashSpeed = 600;
    
    [Header("Tourbillon")]
    [SerializeField] private Transform circleCenter;
    [SerializeField] private int nbOursinsFirstCircle = 8;
    [SerializeField] private float firstCircleRadius = 4;
    [SerializeField] private int nbOursinsSecondCircle = 8;
    [SerializeField] private float secondCircleRadius;
    [SerializeField] private Oursin usedOursin;
    public Vector2[] firstCircle = new Vector2[8];
    public Vector2[] secondCircle = new Vector2[8];
    private int oursinsWave;
    [SerializeField] private Transform roomCenter;
    private bool canTourbillon;
    [SerializeField] private CircleCollider2D tourbillonCollider;

    [Header("Health")] 
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    
    [Header("Visuels")] 
    [SerializeField] private Animator animator;

    private void Start()
    {
        health = maxHealth;
        target = PlayerController.instance.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            PlacementForShootOursin();
        }
    }

    public void PlacementForShootOursin()
    {
        if (transform.position.x <= roomCenter.position.x + 2 &&
            transform.position.x >= roomCenter.position.x - 2 &&
            transform.position.y <= roomCenter.position.y + 2 &&
            transform.position.y >= roomCenter.position.y -2)
        {
            canTourbillon = true;
            rb.velocity = new Vector2(0,0);
            animator.SetBool("Tourbillon", true);
            tourbillonCollider.enabled = true;
        }
        else
        {
            canTourbillon = false;
            Vector2 force = (roomCenter.position - transform.position).normalized * speed;
            rb.AddForce(force, ForceMode2D.Force);
        }
    }

    public void CreateSpawnLists()
    {
        for (int i = 0; i < nbOursinsFirstCircle ; i++)
        {
            Vector2 pos = new Vector2(circleCenter.position.x + Mathf.Cos(2*Mathf.PI/nbOursinsFirstCircle * i) * firstCircleRadius , circleCenter.position.y + Mathf.Sin(2*Mathf.PI/nbOursinsFirstCircle * i) * firstCircleRadius);
            firstCircle[i] = pos;
        }
        
        for (int i = 0; i < nbOursinsSecondCircle ; i++)
        {
            Vector2 pos = new Vector2(circleCenter.position.x + Mathf.Cos(2*Mathf.PI/nbOursinsSecondCircle * i + Mathf.PI/nbOursinsSecondCircle) * secondCircleRadius , circleCenter.position.y + Mathf.Sin(2*Mathf.PI/nbOursinsSecondCircle * i + Mathf.PI/nbOursinsSecondCircle) * secondCircleRadius);
            secondCircle[i] = pos;
        }
    }

    public void SpawnOursins()
    {
        oursinsWave += 1;
        if (oursinsWave == 1)
        {
            for (int i = 0; i < 8; i+=2)
            {
                usedOursin.CannonierShooting(firstCircle[i]);
            }
        }
        if (oursinsWave == 2)
        {
            for (int i = 1; i < 8; i+=2)
            {
                usedOursin.CannonierShooting(firstCircle[i]);
            }
        }
        if (oursinsWave == 3)
        {
            for (int i = 0; i < 8; i+=2)
            {
                usedOursin.CannonierShooting(secondCircle[i]);
            }
        }
        if (oursinsWave == 4)
        {
            for (int i = 1; i < 8; i+=2)
            {
                usedOursin.CannonierShooting(secondCircle[i]);
            }
            tourbillonCollider.enabled = false;
            oursinsWave = 0;
            animator.SetBool("Tourbillon", false);
        }
    }
}
