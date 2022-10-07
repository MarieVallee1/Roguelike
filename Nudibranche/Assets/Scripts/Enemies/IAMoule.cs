using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class IAMoule : MonoBehaviour
{
    // Pathfinding //
    public Transform target;    //TO DO: trouver une autre manière de lui assigner le player que à la main (vraiment utile ?)
    public float speed = 200;
    private Vector2 force;
    private float nextWaypointDistance = 1;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private bool pathUpdated = true;
    private bool stopPathfinding;
    
    // Graph //
    private SpriteRenderer mouleSprite;
    
    // Combat //
    private bool cac;
    private float cacDistance;
    public float timeBetwennAttacks = 1;
    public float timePrepAttack = 1;
    public int damage = 1;
    private Character.CharacterController characterController;  //TO DO: à remplacer par le vrai script du player


    private void Start()
    {
        mouleSprite = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        characterController = target.gameObject.GetComponent<Character.CharacterController>(); 

        InvokeRepeating("UpdatePath", 0, .5f);  // TO DO: à mettre ailleurs pour lui donner une conditions de lancement 
        pathUpdated = true;

    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
           
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void FixedUpdate()
    {
        
        Flip();

        if (!pathUpdated && !stopPathfinding)
        {
            InvokeRepeating("UpdatePath", 0, .5f);
            pathUpdated = true;
        }
        
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position);
        force = direction * speed * Time.deltaTime;

        if (!stopPathfinding)
        {
            rb.AddForce(force);
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private void Update()
    {
        if (stopPathfinding)
        {
            CancelInvoke("UpdatePath");
            seeker.CancelCurrentPathRequest();
            rb.velocity = new Vector2(0, 0);
            path = null;
            pathUpdated = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            cac = true;
            stopPathfinding = true;
            StartCoroutine(Attaque());
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        cac = false;
    }

    private IEnumerator Attaque()
    {
        mouleSprite.color = Color.yellow;
        yield return new WaitForSeconds(timePrepAttack);
        mouleSprite.color = Color.red;
        if (cac)        // TO DO: si parade bien placée pas de dégâts
        {
            characterController.TakeDamage(damage);    // TO DO: récupérer la fonction sur le vrai script du player
        }
        yield return new WaitForSeconds(0.1f);
        mouleSprite.color = Color.white;
        yield return new WaitForSeconds(timeBetwennAttacks);
        if (cac)
        {
            StartCoroutine(Attaque());
        }
        else
        {
            stopPathfinding = false;
        }
    }

    void Flip()
    {
        if (rb.velocity.x >= 0.01f)
        {
            mouleSprite.flipX = true;
        }
        else if (rb.velocity.x <= 0.01f)
        {
            mouleSprite.flipX = false;
        }
    }
} 
