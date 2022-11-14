using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using Pathfinding;

public class IAMoule : MonoBehaviour
{
    // Pathfinding //
    [SerializeField] private Transform target;
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
    [SerializeField] private float repulseSpeed = 100;
    
    // Graph //
    [SerializeField] private GameObject[] visuals;
    [SerializeField] private Animator[] animators;

    // Combat //
    private bool cac;
    public float range = 1;
    public float attackDuration = 1;
    public float timePrepAttack = 1;
    public int damage = 1;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = PlayerController.instance.transform;

        InvokeRepeating("UpdatePath", 0, .5f);  // TO DO: à mettre ailleurs pour lui donner une conditions de lancement 
        pathUpdated = true;
        cac = false;
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
        HandleWalkingSpriteRotation();

        if (cac == false)
        {
            AttaqueRange(); 
        }
        
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Moule") && !cac)
        {
            Vector2 repulseForce = (gameObject.transform.position - col.gameObject.transform.position).normalized * repulseSpeed;
            rb.AddForce(repulseForce, ForceMode2D.Impulse);
        }
    }
    void AttaqueRange()
    {
        if (Vector2.Distance(transform.position, target.position) < range)
        {
            cac = true;
            stopPathfinding = true;
            StartCoroutine(Attaque());
        }
        else
        {
            cac = false;
        }
    }

    private IEnumerator Attaque()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetTrigger("Attack");
        }
        // préparation de l'attaque
        yield return new WaitForSeconds(timePrepAttack);
        // attaque
        if (cac)
        {
            PlayerController.instance.TakeDamage(damage); 
        }
        Debug.Log("coup de la moule");
        
        cac = false;

        yield return new WaitForSeconds(attackDuration);
        if (cac)
        {
            StartCoroutine(Attaque());
        }
        else
        {
            stopPathfinding = false;
        }
    }

    void HandleWalkingSpriteRotation()
    {
        if (Vector2.Angle(Vector2.down, rb.velocity) <= 30)
        {
            visuals[0].SetActive(false);
            visuals[1].SetActive(true);
            visuals[2].SetActive(false);
        }

        if (Vector2.Angle(Vector2.down, rb.velocity) < 150 && Vector2.Angle(Vector2.down, rb.velocity) > 30)
        {
            transform.localScale = new Vector3(1, 1, 1);
            visuals[0].SetActive(true);
            visuals[1].SetActive(false);
            visuals[2].SetActive(false);
            if (Vector2.Angle(Vector2.left, rb.velocity) >= 90)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        if (Vector2.Angle(Vector2.down, rb.velocity) >= 150)
        {
            visuals[0].SetActive(false);
            visuals[1].SetActive(false);
            visuals[2].SetActive(true);
        }
    }
} 
