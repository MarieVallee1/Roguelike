using System;
using System.Collections;
using System.Collections.Generic;
using Character.Skills;
using DG.Tweening;
using Enemies;
using Projectiles;
using Unity.VisualScripting;
using UnityEngine;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerInputActions characterInputs;
        private Rigidbody2D _rb;
        private Transform _tr;
        public static PlayerController instance;
        
        [SerializeField] private Animator[] animator;

        [Header("References")]
        [SerializeField]
        public CharacterData characterData;
        public Projectile usedProjectile;
        [SerializeField] private GameObject book;
        [SerializeField] private Transform bookPos;
        [SerializeField] private GameObject cursor;
        [SerializeField] private Transform parryCooldown;
        [SerializeField] private ParticleSystem parryFeedback;
        [SerializeField] private Transform visualsTr;
        [SerializeField] private GameObject[] visuals;
        [SerializeField] private ParryRepulsion parryRepulsion;
        
        public int currentSkill;
        [SerializeField] private SkillsDetails skills;

        #region Variables
        private Vector2 _direction;
        [HideInInspector] public Vector2 mouseAim;
        [HideInInspector] public Vector2 aim;
        [HideInInspector] public Vector2 characterPos;

        [HideInInspector] public float nextTimeCast;
        
        [Header("Number of Projectile Left")]
        public int blastTracker;
        
        private float _nextTimeParry;
        private float _parryLifeTime;
        private float _skillCountdown;
        private float _blastCooldown;
        public float skillCooldown;
        [Space]
        [SerializeField] private float speedDebug;
        
        public int health;

        [Header("State")]
        public bool canGethit;
        public bool gamepadOn;
        public bool isShooting;
        public bool isParrying;
        public bool isBuffed;
        public bool isMovingUp;
        public bool isMovingDown;
        public bool isFacingLeft;
        public bool isUsingSkill;
        #endregion

        private int _isRunningHash;
        [SerializeField] private bool movementPressed;

        private void Awake()
        {
            if (instance != null && instance != this)
                Destroy(gameObject);

            instance = this;

            characterInputs = new PlayerInputActions();

            _rb = GetComponent<Rigidbody2D>();
            _tr = GetComponent<Transform>();
        }

        private void Start()
        {
            _parryLifeTime = characterData.parryTime;
            _rb.drag = characterData.drag;
            health = characterData.health;
            canGethit = true;
            
            blastTracker = usedProjectile.blastLenght;
            _blastCooldown = usedProjectile.cooldown;
        }

        private void Update()
        {
            characterPos = _tr.position;
            RestrictMousePos();
            Flip();
            HandleSpriteRotation();
            
            _skillCountdown += Time.deltaTime;

            HandleSkillUse();
        }
        private void FixedUpdate()
        {
            //Shoots the projectile
            if(isShooting && blastTracker > 0) usedProjectile.CharacterShooting(this, book.transform.position);
            
            HandleMovement();
            speedDebug = _rb.velocity.magnitude;
            
            AttackCooldown();
            
            if (blastTracker <= 0)
            {
                _blastCooldown -= Time.fixedDeltaTime;
                BlastCooldown(_blastCooldown);
            }

            HandleParry();
            ParryCooldown();
        }
        
        
        private void OnEnable()
        {
            characterInputs.UI.Disable();
            characterInputs.Character.Enable();

            characterInputs.Character.Movement.performed += ctx =>
            {
                _direction = ctx.ReadValue<Vector2>();
                movementPressed = _direction.x != 0 || _direction.y != 0;
                isMovingUp = _direction.y > 0.7;
                isMovingDown = _direction.y < -0.7;
            };
            
            //Allows to detect which controller is used 
            characterInputs.Character.ShootGamepad.performed += ctx =>
            {
                gamepadOn = true;
                isShooting = true;
            };
            characterInputs.Character.ShootGamepad.canceled += ctx => isShooting = false;
            characterInputs.Character.ShootMouse.performed += ctx =>
            {
                gamepadOn = false;
                isShooting = true;
            };
            characterInputs.Character.ShootMouse.canceled += ctx => isShooting = false;
                characterInputs.Character.AimGamepad.performed += ctx =>
            {
                //Disable the cursor when aiming with the gamepad
                cursor.SetActive(false);
                //Handles the direction of the projectile if shot with the gamepad
                aim = ctx.ReadValue<Vector2>();
            };
            
            characterInputs.Character.AimMouse.performed += ctx =>
            {
                //Enable the cursor when shooting with the mouse
                cursor.SetActive(true);
                mouseAim = ctx.ReadValue<Vector2>();
                if(!gamepadOn)aim = new Vector2(mouseAim.x - GameManager.instance.screenWidth / 2, mouseAim.y - GameManager.instance.screenHeight / 2) + characterPos;
            };
            
            characterInputs.Character.Parry.performed += ctx =>
            {
                if (ParryCooldown() && !isParrying) isParrying = true;
                ParryEnemyDetection();
            };
        }
        private void OnDisable()
        {
            characterInputs.Disable();
            movementPressed = false;
        }
        
        
        public void TakeDamage(int damage)
        {
            if (isParrying)
            {
                // Repulsion();
                StartCoroutine(Parry());
                print("I'm Parrying !");
            }
            else
            {

                if (canGethit)
                {
                    //Debugs Death :D 
                    if (health <= 0)
                    {
                        Debug.Log("You're dead");
                        health = characterData.health;
                        health = 0;
                    }
                
                    StartCoroutine(InvulnerabilityFrame());
                    health -= damage;
                
                    //Set the UI to the right amount of hearts
                    Health.instance.SetHealth(health);
                
                    print("I got hit !");
                }

            }
        }

        
        private void HandleMovement()
        {
            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, characterData.maxSpeed);
            
            //Moves the character
            if (movementPressed)
            {
                _rb.AddForce(_direction * characterData.speed,ForceMode2D.Impulse);
                
                for (int i = 0; i < animator.Length; i++)
                {
                    animator[i].SetBool("isRunning", true);
                }
            }
            else
            {
                for (int i = 0; i < animator.Length; i++)
                {
                    animator[i].SetBool("isRunning", false);
                }
            }
        }

        private void ParryEnemyDetection()
        {
            if (characterInputs.Character.Parry.triggered)
            {
                parryRepulsion.enabled = true;
            }
            else
            {
                parryRepulsion.enabled = false;
            }
        }
        private void HandleParry()
        {
            if (isParrying)
            {
                for (int i = 0; i < animator.Length; i++)
                {
                    animator[i].SetBool("isParrying", true);
                }

                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                _rb.velocity = Vector2.zero;
                _parryLifeTime -= Time.deltaTime;

                if(parryFeedback.isStopped) parryFeedback.Play();
            }
            
            //End of parry
            if (_parryLifeTime < 0f)
            {
                parryCooldown.localScale = new Vector3(1, 1, 1);

                var constraints = _rb.constraints;
                constraints = RigidbodyConstraints2D.None;
                constraints = RigidbodyConstraints2D.FreezeRotation;
                _rb.constraints = constraints;

                parryCooldown.DOScale(new Vector3(0, 0, 1),characterData.parryCooldown);
                _nextTimeParry = Time.time + characterData.parryCooldown;
                _parryLifeTime = characterData.parryTime;
                isParrying = false;
                
                for (int i = 0; i < animator.Length; i++)
                {
                    animator[i].SetBool("isParrying", false);
                }
            }
        }
        private IEnumerator Parry()
        {
            parryRepulsion.enabled = true;
            Time.timeScale = 0.7f;
            yield return new WaitForSeconds(0.3f);
            Time.timeScale = 1f;
            parryRepulsion.enabled = false;
            isBuffed = true;
            yield return new WaitForSeconds(characterData.buffDuration);
            isBuffed = false;
        }
        public bool AttackCooldown()
        {
            if(Time.time > nextTimeCast) return true;
            return false;
        }

        public void BlastCooldown(float nextTimeBlast)
        {
            if (nextTimeBlast <= 0)
            {
                blastTracker = usedProjectile.blastLenght;
                _blastCooldown = usedProjectile.cooldown;
            }
        }
        private bool ParryCooldown()
        {
            if(Time.time > _nextTimeParry) return true;
            return false;
        }
        private void HandleSkillUse()
        {
            if(characterInputs.Character.Skill.triggered && _skillCountdown > skillCooldown)
            {
                switch (currentSkill)
                {
                    case 0:
                    {
                        StartCoroutine(skills.SwordSlash());
                    }
                        break;
                    case 1:
                    {
                        skills.WrongTrack(characterPos);
                    }
                        break;
                    case 2:
                    {
                        StartCoroutine(skills.CardLaser(bookPos.position, aim));
                    }
                        break;
                }
            };
        }
        private void Flip()
        {
            visualsTr.transform.localScale = !isFacingLeft ? new Vector3(-1, 1, 1) : new Vector3(1,1,1);
        }
        public void DisableInputs()
        {
            characterInputs.Character.Disable();
            characterInputs.UI.Enable();
            
            //Stop the player from running (anim)
            for (int i = 0; i < animator.Length; i++)
            {
                animator[i].SetBool("isRunning", false);
            }
        }
        public void EnableInputs()
        {
            characterInputs.Character.Enable();
            characterInputs.UI.Disable();
        }

        public void FreezeCharacter()
        {
            _rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        public void UnfreezeCharacter()
        {
            _rb.constraints = RigidbodyConstraints2D.None;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        private void RestrictMousePos()
        {
            Display.RelativeMouseAt(characterPos);
        }
        private IEnumerator InvulnerabilityFrame()
        {
            Debug.Log("invulnerable");
            canGethit = false;
            
            foreach(Transform child in visualsTr)
            {
                SpriteRenderer ren = child.GetComponent<SpriteRenderer>();
                
                ren.DOFade(0, 0.1f);
                yield return new WaitForSeconds(0.1f);
                ren.DOFade(1, 0.1f); 
                yield return new WaitForSeconds(0.1f);
                ren.DOFade(0, 0.1f);
                yield return new WaitForSeconds(0.1f);
                ren.DOFade(1, 0.1f); 
            }

            canGethit = true;
        }
        
        private void HandleSpriteRotation()
        {
            // get the raw angle, in radians
            float radians = Mathf.Atan2 (aim.x, aim.y);
 
            // up to degrees
            float degrees = radians * Mathf.Rad2Deg;

            if(degrees > 55 && degrees < 145)
            {
                isFacingLeft = false;
                visuals[0].SetActive(true);
                visuals[1].SetActive(false);
                visuals[2].SetActive(false);
            }

            if(degrees > 145 || degrees < -145)
            {
                isFacingLeft = true;
                visuals[0].SetActive(false);
                visuals[1].SetActive(false);
                visuals[2].SetActive(true);
            }

            if (degrees > -145 && degrees < -125)
            {
                isFacingLeft = true;
                visuals[0].SetActive(true);
                visuals[1].SetActive(false);
                visuals[2].SetActive(false);
            }

            if (degrees > -125 && degrees < -55)
            {
                isFacingLeft = true;
                visuals[0].SetActive(true);
                visuals[1].SetActive(false);
                visuals[2].SetActive(false);
            }

            if (degrees > -55 && degrees < 35)
            {
                isFacingLeft = false;
                visuals[0].SetActive(false);
                visuals[1].SetActive(true);
                visuals[2].SetActive(false);
            }
        }
    }
}
