using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using Oursins;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    [Header("Comportement de base")] 
    public Transform target;
    private CharacterData characterData;
    [SerializeField] private float speed = 200;
    private Rigidbody2D rb;
    [SerializeField] private Transform princessFeet;

    public enum Behaviour
    {
        walk,
        hit,
        rush,
        oursins,
    }

    public Behaviour behaviour;
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
    [SerializeField] private Collider2D bossCollider;
    [SerializeField] private Collider2D tourbillonCollider;

    [Header("Health")] 
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    public Slider healthGauge;
    private Vector2 rushTarget;
    private float shaderDissolveValue = 1;
    public float dissolveDuration = 5;
    private bool dead;
    [HideInInspector] public bool vulnerable;

    [Header("Visuels")] 
    [SerializeField] private Animator[] animators;
    [SerializeField] private GameObject[] visuals;
    [SerializeField] private ParticleSystem vfxDamage;
    [SerializeField] private SpriteRenderer[] sprites;
    [SerializeField] private Material[] charaMat;
    
    //Activation
    [SerializeField] private Collider2D projectileHit;
    
    //Audio
    [SerializeField] private AudioSource princessHit;
    [SerializeField] private AudioSource urchinLaunch;
    [SerializeField] private AudioSource princessDash;
    [SerializeField] private AudioSource princessSwing;

    private void Start()
    {
        GameManager.instance.bossFightOn = true;
        GameManager.instance.bossScript = this;
        health = maxHealth;
        target = PlayerController.Instance.transform.GetChild(5);
        characterData = PlayerController.Instance.characterData;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        projectileHit.enabled = true;
        bossCollider.enabled = true;
        healthGauge = GameManager.instance.bossGauge;
        healthGauge.gameObject.SetActive(true);
        behaviour = Behaviour.walk;
        firstCircle = new Vector2[nbOursinsFirstCircle];
        secondCircle = new Vector2[nbOursinsSecondCircle];
        thirdCircle = new Vector2[nbOursinsThirdCircle];
        healthGauge.maxValue = maxHealth;
        healthGauge.value = Single.MaxValue;
        dead = false;
        vulnerable = true;
    }

    private void Update()
    {
        if (dead)
        {
            GameManager.instance.bossFightOn = false;
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].material.SetFloat("_Dissolve", shaderDissolveValue);
            }
        }
    }

    private void FixedUpdate()
    {
        Timer();
        
        if (behaviour == Behaviour.walk && !dead)
        {
            Vector2 force = (target.transform.position - princessFeet.transform.position).normalized * (speed * Time.deltaTime);
            rb.AddForce(force, ForceMode2D.Force);
            HandleSpriteRotation(target.position - princessFeet.position);
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
                rushTarget = target.position;
                timerForRush = 0;
            }
            
            if (Vector2.Distance(target.position, princessFeet.position) <= attackRange && timerForAttack >= attackTimer)
            {
                behaviour = Behaviour.hit;
                timerForAttack = 0;
            }
        }

        if (behaviour == Behaviour.hit)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetBool("Attaque", true);
            }
            if (Vector2.Distance(target.position, princessFeet.position) > attackRange)
            {
                behaviour = Behaviour.walk;
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].SetBool("Attaque", false);
                }
            }
            else
            {
                if (timerForAttack >= attackTimer)
                {
                    for (int i = 0; i < animators.Length; i++)
                    {
                        animators[i].SetBool("Attaque", true);
                    }
                    timerForAttack = 0;
                }
            }
        }
        
        if (behaviour == Behaviour.rush)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetBool("Ruée", true);
            }
            Vector2 force = (rushTarget - (Vector2)princessFeet.transform.position).normalized * (rushSpeed * Time.deltaTime);
            rb.AddForce(force, ForceMode2D.Force);

            if (Vector2.Distance(rushTarget, princessFeet.position) <= 1 )
            {
                behaviour = Behaviour.hit;
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].SetBool("Attaque", true);
                }
                timerForAttack = 0;
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].SetBool("Ruée", false);
                }
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
            transform.position.y <= roomCenter.position.y &&
            transform.position.y >= roomCenter.position.y - 1f)
        {
            rb.drag = 100;
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetBool("Attaque", false);
            }
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetBool("Tourbillon", true);
            }

            bossCollider.enabled = false;
            tourbillonCollider.enabled = true;
            canTourbillon = false;
        }
        else
        {
            Vector2 force = (roomCenter.position - transform.position).normalized * rushSpeed * Time.deltaTime;
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
        urchinLaunch.PlayOneShot(AudioList.Instance.urchinLaunch);

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
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetBool("Tourbillon", false);
            }
            rb.drag = 1.5f;
            bossCollider.enabled = true;
            tourbillonCollider.enabled = false;
            behaviour = Behaviour.walk;
        }
    }

    public void TakeDamage(int damage)
    {
        if (vulnerable)
        {
            StartCoroutine(HitFeedback());
        
            health -= damage;
            vfxDamage.Play();
            healthGauge.value = health;

            tourbillonCount += 1;

            if (!dead)
            {
                if (health <= 0)
                {
                    PlayerController.Instance.vulnerable = false;
                    healthGauge.gameObject.SetActive(false);
                    dead = true;
                    vulnerable = false;
                    DeathBeginning();
                }
            }
        
            if (tourbillonCount == tourbillonEveryXDamages)
            {
                canTourbillon = true;
                behaviour = Behaviour.oursins;
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].SetBool("Ruée", false);
                }
                tourbillonCount = 0;
            }
        }
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

    private void DeathBeginning()
    {
        PlayerController.Instance.DisableInputs();
        PlayerController.Instance._rb.constraints = RigidbodyConstraints2D.FreezeAll;
        PlayerController.Instance.movementPressed = false;
        GameManager.instance.bossCinematicPos = transform.position;
        GameManager.instance.inBossCutscene = true;
        GameManager.instance.bossKilled = true;
        AudioList.Instance.bossKilled = true;
        AudioList.Instance.StartMusic(AudioList.Music.ending,true);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        visuals[0].SetActive(false);
        visuals[1].SetActive(true);
        visuals[2].SetActive(false);
        animators[1].SetTrigger("Mort");
    }
    
    public IEnumerator Death()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].material = charaMat[0];
        }
        
        DOTween.To(()=> shaderDissolveValue, x=> shaderDissolveValue = x, -1, dissolveDuration);
        yield return new WaitForSeconds(dissolveDuration-3);
        ConclusionCinematic.instance.StartConclusionCinematic();
    }
    
    public void StartCoroutineTarget(Transform baitTransform)
    {
        StartCoroutine(ChangeTarget(baitTransform));
    }

    private IEnumerator ChangeTarget(Transform baitTransform)
    {
        target = baitTransform;
        yield return new WaitForSeconds(characterData.baitDuration);
        target = PlayerController.Instance.transform.GetChild(5);
    }
    
    private IEnumerator HitFeedback()
    {
        princessHit.PlayOneShot(AudioList.Instance.enemyHit);
        
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].material = charaMat[1];
        }

        yield return new WaitForSeconds(0.1f);
            
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].material = charaMat[0];
        }
            
        yield return new WaitForSeconds(0.1f);
    }

    public void PlayDashSound()
    {
        Debug.Log("Dash");
        princessDash.PlayOneShot(AudioList.Instance.basicAttack);
    }

    public void PlaySlashSound()
    {
        Debug.Log("Slash");
        princessSwing.PlayOneShot(AudioList.Instance.mouleSword);
    }
}
