using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using Pathfinding;
using Pathfinding.Util;
using CrevetteProjectiles;

namespace Ennemy
{
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
    [SerializeField] private float dontPushDistance = 0.01f;

    private bool StopPathfinding
    {
        get { return stopPathfinding; }

        set
        {
            if (value != stopPathfinding)
            {
                if (!stopPathfinding)
                {
                    rb.velocity = new Vector2(0, 0);
                }
            }

            stopPathfinding = value;
        }
    }
    
    //Graph//
    [SerializeField] private GameObject[] visuals;
    [SerializeField] private Animator[] animators;
    
    //Combat//
    public float projectileDiameter;
    [SerializeField] private CrevetteProjectile usedCrevetteProjectile;
    private bool attaque;
    public float nextTimeCast;

    //Health//
    public int pv = 5;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        target = PlayerController.instance.transform.GetChild(6);
        
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
        AttackCooldown();

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
            path = null;
            pathUpdated = false;
        }

        if (Vector2.Distance(transform.position, target.transform.position) <= targetDistance)
        {
            Vector2 raycastDirection = (target.transform.position) - transform.position;
            RaycastHit2D raycastHit2D = Physics2D.BoxCast(transform.position,
                new Vector2(projectileDiameter, projectileDiameter), Vector2.Angle(Vector2.right, raycastDirection),
                raycastDirection, raycastDirection.magnitude, LayerMask.GetMask("ProjectileHitPlayer", "Obstacle"));
            if (raycastHit2D.collider.gameObject.layer == 13)
            {
                StopPathfinding = true;
                attaque = true;
            }
            else
            {
                StopPathfinding = false;
                attaque = false;
            }
            Debug.DrawRay(transform.position, raycastDirection, Color.red);
        }
        else
        {
            StopPathfinding = false;
            attaque = false;
        }
        
        if (attaque)
        {
            usedCrevetteProjectile.CrevetteShooting(this, this.transform.position, target.position - transform.position);
        }
        
        HandleSpriteRotation(rb.velocity);
    }

    public bool AttackCooldown()
    {
        if(Time.time > nextTimeCast) return true;
        return false;
    }
    
    void HandleSpriteRotation(Vector2 direction)
    {
        if (Vector2.Angle(Vector2.down, direction) <= 30)
        {
            visuals[0].SetActive(false);
            visuals[1].SetActive(true);
            visuals[2].SetActive(false);
        }

        if (Vector2.Angle(Vector2.down, direction) < 150 && Vector2.Angle(Vector2.down, rb.velocity) > 30)
        {
            transform.localScale = new Vector3(1, 1, 1);
            visuals[0].SetActive(true);
            visuals[1].SetActive(false);
            visuals[2].SetActive(false);
            
            if (Vector2.Angle(Vector2.left, direction) >= 90)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if (Vector2.Angle(Vector2.down,direction) >= 150)
        {
            visuals[0].SetActive(false);
            visuals[1].SetActive(false);
            visuals[2].SetActive(true);
        }
    }
}
}

