using System;
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
    private CharacterData characterData;
    public float speed = 150;
    public float speedOutOfCamera = 50;
    private Vector2 force;
    private float nextWaypointDistance = 1;
    private Path path;
    private int currentWaypoint;
    private bool reachedEndOfPath;
    private Seeker seeker;
    private Rigidbody2D rb;
    private bool pathUpdated = true;
    public bool stopPathfinding = false;
    public float targetDistance = 1;
    [SerializeField] private float dontPushDistance = 0.01f;
    private bool isWalking;
    private Camera mainCam;
    private bool isVisible;

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
    [SerializeField] private Transform faceArme;
    [SerializeField] private Transform coteArme;
    [SerializeField] private Transform dosArme;
    public bool spriteRotation = true;

    //Combat//
    public float projectileDiameter;
    [SerializeField] private CrevetteProjectile usedCrevetteProjectile;
    private bool attaque;
    private Transform shotOrigin;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        target = PlayerController.Instance.transform.GetChild(6);
        characterData = PlayerController.Instance.characterData;
        
        InvokeRepeating("UpdatePath", 0, .5f);
        pathUpdated = true;
        mainCam = GameManager.instance.mainCamera;
        shotOrigin = faceArme;
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
        
        if (isVisible)
        {
            force = direction * speed * Time.deltaTime;
        }
        else
        {
            force = direction * speedOutOfCamera * Time.deltaTime;
        }

        if (!stopPathfinding)
        {
            rb.AddForce(force);
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
        VisibleByCamera();
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
        
        CalculateShootRange();
    }
    
    void VisibleByCamera()
    {
        Vector2 viewPos = mainCam.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            isVisible = true;
        }
    }

    void CalculateShootRange()
    {
        if (Vector2.Distance(transform.position, target.transform.position) <= targetDistance)
        {
            Vector2 raycastDirection = (target.transform.position) - transform.position;
            RaycastHit2D raycastHit2D = Physics2D.BoxCast(shotOrigin.position,
                new Vector2(projectileDiameter, projectileDiameter), Vector2.Angle(Vector2.right, raycastDirection),
                raycastDirection, raycastDirection.magnitude, LayerMask.GetMask("ProjectileHitPlayer", "Obstacle"));
            Debug.DrawRay(shotOrigin.position, raycastDirection, Color.red);
            if (raycastHit2D.collider.gameObject.layer == 8)
            {
                StopPathfinding = false;
                attaque = false;
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].SetBool("Attack", false);
                }
                
                HandleSpriteRotation(rb.velocity);
            }
            else if (raycastHit2D.collider.gameObject.layer == 13)
            {
                StopPathfinding = true;
                attaque = true;
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].SetBool("Attack", true);
                }
            }
        }
        else
        {
            StopPathfinding = false;
            attaque = false;
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetBool("Attack", false);
            }
            HandleSpriteRotation(rb.velocity);  
            
        } 
    }

    public void Shoot()
    {
        HandleSpriteRotation((target.position - transform.position).normalized);
        
        usedCrevetteProjectile.CrevetteShooting(this, this.shotOrigin.position, target.position - transform.position);
    }

    void HandleSpriteRotation(Vector2 direction)
    {
        Debug.DrawRay(transform.position, direction * 3, Color.black);
        Debug.DrawRay(transform.position, Vector3.down*3, Color.magenta);
        if (spriteRotation)
        {
            if (Vector2.Angle(Vector2.down, direction) <= 30)
            {
                visuals[0].SetActive(false);
                visuals[1].SetActive(true);
                visuals[2].SetActive(false);
                
                transform.localScale = new Vector3(1, 1, 1);
                shotOrigin = faceArme;
            }
            else
            if (Vector2.Angle(Vector2.down, direction) < 150 && Vector2.Angle(Vector2.down, direction) > 30)
            {
                transform.localScale = new Vector3(1, 1, 1);
                visuals[0].SetActive(true);
                visuals[1].SetActive(false);
                visuals[2].SetActive(false);
                
                shotOrigin = coteArme;

                if (Vector2.Angle(Vector2.left, direction) >= 90)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else
            if (Vector2.Angle(Vector2.down,direction) >= 150)
            {
                visuals[0].SetActive(false);
                visuals[1].SetActive(false);
                visuals[2].SetActive(true);
                
                transform.localScale = new Vector3(1, 1, 1);
                
                shotOrigin = dosArme;
            }
        }
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

