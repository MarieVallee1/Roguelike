using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oursins;

public class Boss : MonoBehaviour
{
    [Header("Comportement de base")]
    [SerializeField] private float speed = 200;
    
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

    [Header("Health")] 
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    
    [Header("Visuels")] 
    [SerializeField] private Animator animator;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("Tourbillon");
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
            oursinsWave = 0;
        }
    }
}
