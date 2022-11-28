using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using Oursins;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("Comportement de base")] 
    public Transform target;
    [SerializeField] private float speed = 200;
    private Rigidbody2D rb;
    [SerializeField] private Transform princessFeet;

    private enum Behaviour
    {
        walk,
        hit,
        rush,
        oursins,
    }

    [SerializeField]private Behaviour behaviour;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private float attackTimer = 2;
    private float timerForAttack;

    [Header("Ruée")]
    [SerializeField] private float rushSpeed = 600;
    [SerializeField] private float rushRange = 6;
    [SerializeField] private float rushTimer = 2;
    private float timerForRush = 0;

    [Header("Tourbillon")] public int tourbillonEveryXDamages = 10;
    [SerializeField] private Transform circleCenter;
    [SerializeField] private int nbOursinsFirstCircle = 8;
    [SerializeField] private float firstCircleRadius = 4;
    [SerializeField] private int nbOursinsSecondCircle = 8;
    [SerializeField] private float secondCircleRadius;
    [SerializeField] private int nbOursinsThirdCircle = 8;
    [SerializeField] private float thirdCircleRadius;
    [SerializeField] private Oursin usedOursin;
    public Vector2[] firstCircle;
    public Vector2[] secondCircle;
    public Vector2[] thirdCircle;
    private int oursinsWave;
    [SerializeField] private Transform roomCenter;
    private bool canTourbillon;
    private int tourbillonCount;

    [Header("Health")] 
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    private Slider healthGauge;
    private Vector2 rushTarget;
    
    [Header("Visuels")] 
    [SerializeField] private Animator animator;

    [SerializeField] private ParticleSystem vfxDamage;

    private void Start()
    {
        health = maxHealth;
        target = PlayerController.Instance.transform.GetChild(6);
        rb = GetComponent<Rigidbody2D>();
        healthGauge = GameManager.instance.bossGauge;
    }

    private void OnEnable()
    {
        healthGauge.gameObject.SetActive(true);
        behaviour = Behaviour.walk;
        firstCircle = new Vector2[nbOursinsFirstCircle];
        secondCircle = new Vector2[nbOursinsSecondCircle];
        thirdCircle = new Vector2[nbOursinsThirdCircle];
        healthGauge.maxValue = maxHealth;
        healthGauge.value = Single.MaxValue;
    }

    private void FixedUpdate()
    {
        Timer();
        
        if (behaviour == Behaviour.walk)
        {
            Vector2 force = (target.transform.position - princessFeet.transform.position).normalized * (speed * Time.deltaTime);
            rb.AddForce(force, ForceMode2D.Force);
        }
        
        if (canTourbillon)
        {
            PlacementForShootOursin();
        }
        
        if (behaviour == Behaviour.walk)
        {
            if (Vector2.Distance(target.position, princessFeet.position) >= rushRange && timerForRush >= rushTimer)
            {
                behaviour = Behaviour.rush;
                animator.SetBool("Ruée", true);
                rushTarget = target.position;
                timerForRush = 0;
            }
            
            if (Vector2.Distance(target.position, princessFeet.position) <= attackRange)
            {
                behaviour = Behaviour.hit;
                animator.SetTrigger("Attaque");
                timerForAttack = 0;
            }
        }

        if (behaviour == Behaviour.hit)
        {
            if (Vector2.Distance(target.position, princessFeet.position) > attackRange)
            {
                behaviour = Behaviour.walk;
            }
            else
            {
                if (timerForAttack >= attackTimer)
                {
                    animator.SetTrigger("Attaque");
                    timerForAttack = 0;
                }
            }
        }
        
        if (behaviour == Behaviour.rush)
        {
            Vector2 force = (rushTarget - (Vector2)princessFeet.transform.position).normalized * rushSpeed * Time.deltaTime;
            rb.AddForce(force, ForceMode2D.Force);

            if (Vector2.Distance(rushTarget, princessFeet.position) <= 1)
            {
                animator.SetBool("Ruée", false);
                behaviour = Behaviour.hit;
                animator.SetTrigger("Attaque");
                timerForAttack = 0;
            }
        }
    }


    void Timer()
    {
        timerForRush += Time.deltaTime;
        timerForAttack += Time.deltaTime;
    }

    public void PlacementForShootOursin()
    {
        if (transform.position.x <= roomCenter.position.x + 0.5f &&
            transform.position.x >= roomCenter.position.x - 0.5f &&
            transform.position.y <= roomCenter.position.y + 0.5f &&
            transform.position.y >= roomCenter.position.y - 0.5f)
        {
            rb.drag = 100;
            animator.SetBool("Tourbillon", true);
            canTourbillon = false;
        }
        else
        {
            Vector2 force = (roomCenter.position - transform.position).normalized * speed * Time.deltaTime;
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
        
        for (int i = 0; i < nbOursinsThirdCircle ; i++)
        {
            Vector2 pos = new Vector2(circleCenter.position.x + Mathf.Cos(2*Mathf.PI/nbOursinsThirdCircle * i) * thirdCircleRadius , circleCenter.position.y + Mathf.Sin(2*Mathf.PI/nbOursinsThirdCircle * i) * thirdCircleRadius);
            thirdCircle[i] = pos;
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
                usedOursin.CannonierShooting(thirdCircle[i]);
            }
        }
        if (oursinsWave == 2)
        {
            for (int i = 1; i < 8; i+=2)
            {
                usedOursin.CannonierShooting(firstCircle[i]);
                usedOursin.CannonierShooting(thirdCircle[i]);
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
            animator.SetBool("Tourbillon", false);
            rb.drag = 1.5f;
            behaviour = Behaviour.walk;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        vfxDamage.Play();
        healthGauge.value = health;

        tourbillonCount += 1;
        if (tourbillonCount == tourbillonEveryXDamages)
        {
            canTourbillon = true;
            behaviour = Behaviour.oursins;
            animator.SetBool("Ruée", false);
            tourbillonCount = 0;
        }

        if (health <= 0)
        {
            healthGauge.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
