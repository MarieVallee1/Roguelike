using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.Util;

public class Crevette : MonoBehaviour
{
    //Pathfinding//
    public Transform target;
    public float speed = 150;
    private Vector2 force;
    private float nextWaypointDistance = 1;
    private Path path;
    private int currentWaypoint;
    private bool reachedEndOfPath;
    private Seeker seeker;
    private Rigidbody2D rb;
    private bool pathUpdated = true;
    private bool stopPathfinding = false;
    public float targetDistance = 1;
    
    //Graph//
    private SpriteRenderer crevetteSprite;
    
    //Combat//
    public int damage = 1;
    public float projectileDiameter;
    
    //Health//
    public int pv = 5;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        crevetteSprite = GetComponent<SpriteRenderer>();
        
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

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
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
    
    void Update()
    {
        if (stopPathfinding)
        {
            CancelInvoke("UpdatePath");
            seeker.CancelCurrentPathRequest();
            rb.velocity = new Vector2(0, 0);
            path = null;
            pathUpdated = false;
        }

        if (Vector2.Distance(transform.position, target.transform.position) <= targetDistance)
        {
            Vector2 raycastDirection = target.transform.position - transform.position;
            if (Physics2D.BoxCast(transform.position, new Vector2(projectileDiameter, projectileDiameter), Vector2.Angle(Vector2.right, raycastDirection), raycastDirection, raycastDirection.magnitude,
                    LayerMask.GetMask("Obstacle")))
            {
                stopPathfinding = false;
            }
            else
            {
                stopPathfinding = true; 
            }
            Debug.DrawRay(transform.position, raycastDirection, Color.red);
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
            crevetteSprite.flipX = false;
        }
        else if (rb.velocity.x <= 0.01f)
        {
            crevetteSprite.flipX = true;
        }
    }
}
