using System;
using Character;
using UnityEngine;
using Pathfinding;

public class IAMoule : MonoBehaviour
{
    // Pathfinding //
    [SerializeField] private Transform target;
    public float speed = 200;
    public float speedOutOfCamera = 100;
    private Vector2 force;
    private float nextWaypointDistance = 1;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private bool pathUpdated = true;
    public bool stopPathfinding;
    [SerializeField] private float repulseSpeed = 100;
    public Transform mouleFeet;
    private Camera mainCam;
    private bool isVisible;
    

    // Graph //
    [SerializeField] private GameObject[] visuals;
    [SerializeField] private Animator[] animators;

    // Combat //
    private bool cac;
    public float range = 1;
    public int damage = 1;
    private bool isAttacking;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        //target = PlayerController.instance.transform;
        target = PlayerController.instance.transform.GetChild(6);

        InvokeRepeating("UpdatePath", 0, .5f);  // TO DO: à mettre ailleurs pour lui donner une conditions de lancement 
        pathUpdated = true;
        cac = false;
        mainCam = GameManager.instance.mainCamera;
    }

    private void OnEnable()
    {
        isVisible = false;
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
        
        ParryBehavior(PlayerController.instance);
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
        
        if (!isAttacking)
        {
            HandleSpriteRotation(rb.velocity);
        }

        AttaqueRange();

        VisibleByCamera();

        //Freeze and Unfreeze the enemy when the player is using a skill
        if (PlayerController.instance.isUsingSkill) speed -= speed;
    }

    void VisibleByCamera()
    {
        Vector2 viewPos = mainCam.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            isVisible = true;
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
        if (Physics2D.Raycast(mouleFeet.position, target.position - mouleFeet.position, range,
                LayerMask.GetMask("Player")))
        {
            if (!isAttacking)
            {
                HandleSpriteRotation(target.position - mouleFeet.position); 
            }
            cac = true;
            stopPathfinding = true;
            isAttacking = true;
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetBool("Attack", true);
            }
        }
        else
        {
            cac = false;
        }
    }
    public void InflictDamages()
    {
        if (cac)
        {
            PlayerController.instance.TakeDamage(damage); 
        }
    }

    public void AttackEnded()
    {
        if (!cac)
        {
            isAttacking = false;
            stopPathfinding = false;
            
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetBool("Attack", false);
            }
            return;
        }
        
        HandleSpriteRotation(target.position - mouleFeet.position);
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
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        if (Vector2.Angle(Vector2.down,direction) >= 150)
        {
            visuals[0].SetActive(false);
            visuals[1].SetActive(false);
            visuals[2].SetActive(true);
        }
    }

    void ParryBehavior(PlayerController pc)
    {
        if (cac && pc.isParrying)
        {
            Debug.Log("YES");
            Vector2 dir = pc.characterPos - (Vector2)transform.position;
            rb.AddForce(-dir * pc.characterData.repulsionForce,ForceMode2D.Impulse);
        }
    }
} 
