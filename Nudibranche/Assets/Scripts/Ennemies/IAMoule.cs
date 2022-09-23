using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class IAMoule : MonoBehaviour
{
    private SpriteRenderer mouleSprite;

    public Transform target;
    public float speed = 200;
    private Vector2 force;
    private float nextWaypointDistance = 1;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;

    private bool cac = false;
    public float cacDistanceMax = 1;
    private float cacDistance;

    private bool pathUpdated = false;

    private void Start()
    {
        mouleSprite = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0, .5f);
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

        if (!cac)
        {
            if (!pathUpdated)
            {
                Debug.Log("Relancer pathfinding");
                //pathUpdated = true;
            }
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
        cacDistance = Vector2.Distance(gameObject.transform.position, target.transform.position);
        if (cacDistance<= cacDistanceMax)
        {
            cac = true;
        }
        else
        {
            cac = false;
        }

        if (cac)
        {
            CancelInvoke("UpdatePath");
            seeker.CancelCurrentPathRequest();
            rb.velocity = new Vector2(0, 0);
            path = null;
            pathUpdated = false;
        }
        
        Debug.Log(cac);
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
