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
    [SerializeField]private Rigidbody2D rb;
    private bool pathUpdated = true;
    public bool stopPathfinding;
    [SerializeField] private float repulseSpeed = 100;
    public Transform mouleFeet;
    private Camera mainCam;
    private bool isVisible;

    // Graph //
    [SerializeField] private GameObject[] visuals;
    [SerializeField] private Animator[] animators;
    [SerializeField] private ParticleSystem[] attackVFX;

    // Combat //
    private bool cac;
    public float range = 1;
    public int damage = 1;
    private bool isAttacking;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        target = PlayerController.Instance.transform.GetChild(6);

        InvokeRepeating("UpdatePath", 0, .5f);  
        pathUpdated = true;
        cac = false;
        mainCam = GameManager.instance.mainCamera;
    }

    private void OnEnable()
    {
        isVisible = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
    }

    private void Update()
    {
        if (stopPathfinding)
        {
            CancelInvoke("UpdatePath");
            seeker.CancelCurrentPathRequest();
            //rb.velocity = new Vector2(0, 0);
            path = null;
            pathUpdated = false;
        }
        
        AttaqueRange();

        if (!isAttacking)
        {
            HandleSpriteRotation(rb.velocity);
        }

        VisibleByCamera();

        //Freeze and Unfreeze the enemy when the player is using a skill
        if (PlayerController.Instance.onSkillUse) speed -= speed;
        
        OutOfRange();
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
            if (!isAttacking)
            {
                cac = false;
            }
        }
    }
    public void InflictDamages()
    {
        if (cac)
        {
            PlayerController.Instance.TakeDamage(damage); 
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

    private void OutOfRange()
    {
        if(!cac) stopPathfinding = false;
    }

    void HandleSpriteRotation(Vector2 direction)
    {
        if (Vector2.Angle(Vector2.down, direction) <= 30)
        {
            visuals[0].SetActive(false);
            visuals[1].SetActive(true);
            visuals[2].SetActive(false);
        }

        if (Vector2.Angle(Vector2.down, direction) < 150 && Vector2.Angle(Vector2.down, direction) > 30)
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

    public void AttackTiming()
    {
        //Set the moment when the player can parry the enemy's attack
        isAttacking = !isAttacking;
    }
    public void PlayAttackVFX()
    {
        for (int i = 0; i < attackVFX.Length; i++)
        {
            attackVFX[i].Play();
        }
    }
} 
