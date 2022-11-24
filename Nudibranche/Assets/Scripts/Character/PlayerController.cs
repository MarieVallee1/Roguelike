using System;
using System.Collections;
using Character.Skills;
using DG.Tweening;
using UnityEngine;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        public static PlayerController Instance;
        
        [Header("Character System Related")]
        private Rigidbody2D _rb;
        private Transform _tr;
        public PlayerInputActions characterInputs;
        public CharacterData characterData;
        [SerializeField] private SkillsDetails skills;
        [SerializeField] private GameObject cursor;
        
        [Header("Book Related")]
        [SerializeField] private GameObject book;
        [SerializeField] private Transform bookPos;
        [SerializeField] private Animator bookAnim;
        
        [Header("Parry Related")]
        [SerializeField] private Transform parryCooldown;
        [SerializeField] private ParticleSystem parryFeedback;
        [SerializeField] private ParryRepulsion parryRepulsion;
        
        [Header("Character Visuals Related")]
        [SerializeField] private Transform characterVisualsTr;
        [SerializeField] private GameObject[] characterFaces;
        [SerializeField] private Animator[] animator;


        #region Variables
        
        private Vector2 _movementDirection;
        [HideInInspector] public Vector2 mouseAim;
        [HideInInspector] public Vector2 aim;
        [HideInInspector] public Vector2 characterPos;
        [HideInInspector] public Vector2 shootDir;
        
        [HideInInspector] public float nextTimeShoot;
        
        [Header("Number of Projectile Left")]
        public int remainingProjectile;

        [Header("Equipped Skill")]
        [SerializeField] private string currentSkill;
        [HideInInspector] public int skillIndex;

        private float _nextTimeParry;
        private float _parryLifeTime;
        private float _skillCountdown;
        private float _blastCooldown;
        
        [Header("Stats")]
        public float skillCooldown;
        public int health;
        public float speed;
        public float damage;
        public float fireRate;
        public float projectileSize;
        

        [Header("State")]
        public bool gamepadOn;
        [Space]
        public bool vulnerable;
        public bool onShoot;
        public bool onParry;
        public bool onBuff;
        public bool onSkillUse;
        [Space]
        public bool movementPressed;
        public bool isMovingUp;
        public bool isMovingDown;
        public bool isFacingLeft;
        
        #endregion

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);

            Instance = this;

            characterInputs = new PlayerInputActions();

            _rb = GetComponent<Rigidbody2D>();
            _tr = GetComponent<Transform>();
        }
        private void Start()
        {
            _parryLifeTime = characterData.parryTime;
            _rb.drag = characterData.drag;
            health = characterData.health;
            vulnerable = true;
            remainingProjectile = characterData.usedProjectile[characterData.projectileIndex].blastLenght;
            _blastCooldown = characterData.usedProjectile[characterData.projectileIndex].blastCooldown;
        }

        
        private void OnEnable()
        {
            characterInputs.UI.Disable();
            characterInputs.Character.Enable();

            characterInputs.Character.Movement.performed += ctx =>
            {
                _movementDirection = ctx.ReadValue<Vector2>();
                movementPressed = _movementDirection.x != 0 || _movementDirection.y != 0;
                isMovingUp = _movementDirection.y > 0.7;
                isMovingDown = _movementDirection.y < -0.7;
            };
            
            //Allows to detect which controller is used 
            characterInputs.Character.ShootGamepad.performed += ctx =>
            {
                gamepadOn = true;
                onShoot = true;
            };
            characterInputs.Character.ShootGamepad.canceled += ctx => onShoot = false;
            characterInputs.Character.ShootMouse.performed += ctx =>
            {
                gamepadOn = false;
                onShoot = true;
            };
            characterInputs.Character.ShootMouse.canceled += ctx => onShoot = false;
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
                
                //Adapt the aim of the mouse to the screen size
                if(!gamepadOn) aim = new Vector2(mouseAim.x - GameManager.instance.screenWidth / 2, mouseAim.y - GameManager.instance.screenHeight / 2) + characterPos;
            };
            
            characterInputs.Character.Parry.performed += ctx =>
            {
                if (ParryCooldown() && !onParry) onParry = true;
            };
        }
        private void OnDisable()
        {
            characterInputs.Disable();
            movementPressed = false;
        }
        
        
        private void Update()
        {
            RestrictMousePos();
            Flip();
            HandleSpriteRotation();
            ParryCooldown();
            AttackCooldown();
            
            if (remainingProjectile <= 0)
            {
                _blastCooldown -= Time.deltaTime;
                BlastCooldown(_blastCooldown);
            }
            
            characterPos = _tr.position;
            _skillCountdown += Time.deltaTime;
        }
        private void FixedUpdate()
        {
            //Shoots the projectile
            if (onShoot && remainingProjectile > 0) Shoot();
            else bookAnim.SetBool("isShooting", false);

            HandleMovement();
            HandleParry();
            HandleSkillUse();
        }
        
        

        
        
        public void TakeDamage(int damage)
        {
            if (onParry)
            {
                StartCoroutine(Parry());
                print("I'm Parrying !");
            }
            else
            {

                if (vulnerable)
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
                if(!onBuff)speed = characterData.speed;
                else speed = characterData.speedBuff;
                
                _rb.AddForce(_movementDirection * speed,ForceMode2D.Impulse);
                
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

        
        private void HandleParry()
        {
            if (onParry)
            {
                for (int i = 0; i < animator.Length; i++)
                {
                    animator[i].SetBool("isParrying", true);
                }

                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                _rb.velocity = Vector2.zero;
                _parryLifeTime -= Time.deltaTime;
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
                onParry = false;
                
                for (int i = 0; i < animator.Length; i++)
                {
                    animator[i].SetBool("isParrying", false);
                }
            }
        }
        private IEnumerator Parry()
        {
            //Detect enemies near
            parryRepulsion.enabled = true;
            yield return new WaitForSeconds(0.2f);
            //Stop detecting enemies
            parryRepulsion.enabled = false;
                
            //Unfreeze Character
            var constraints = _rb.constraints;
            constraints = RigidbodyConstraints2D.None;
            constraints = RigidbodyConstraints2D.FreezeRotation;
            _rb.constraints = constraints;
            
            //Play VFX
            if(parryFeedback.isStopped) parryFeedback.Play();
            
            //Slow Time
            Time.timeScale = 0.7f;
            yield return new WaitForSeconds(0.3f);
            Time.timeScale = 1f;
            
            //Activate Buff
            onBuff = true;
            yield return new WaitForSeconds(characterData.buffDuration);
            onBuff = false;
        }

        private void Shoot()
        {
            GameObject usedProjectile = PoolingSystem.instance.GetObject(characterData.usedProjectile[characterData.projectileIndex].usedProjectileName);

            if (usedProjectile != null && AttackCooldown())
            { 
                //Camera Shake
                //CinemachineShake.instance.ShakeCamera(0.3f,0.1f);
                
                //Anim
                bookAnim.SetBool("isShooting", true);
                
                //Placement & activation
                usedProjectile.transform.position = book.transform.position;
                usedProjectile.SetActive(true);
                shootDir = aim.normalized;
            
                //Physic
                usedProjectile.GetComponent<Rigidbody2D>().velocity = shootDir * characterData.usedProjectile[characterData.projectileIndex].projectileSpeed;
                

                nextTimeShoot = Time.time + characterData.usedProjectile[characterData.projectileIndex].fireRate;
                remainingProjectile -= 1;
            }
        }
        public bool AttackCooldown()
        {
            if(Time.time > nextTimeShoot) return true;
            return false;
        }
        public void BlastCooldown(float nextTimeBlast)
        {
            if (nextTimeBlast <= 0)
            {
                remainingProjectile = characterData.usedProjectile[characterData.projectileIndex].blastLenght;
                _blastCooldown = characterData.usedProjectile[characterData.projectileIndex].blastCooldown;
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
                switch (skillIndex)
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
            characterVisualsTr.transform.localScale = !isFacingLeft ? new Vector3(-1, 1, 1) : new Vector3(1,1,1);
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
            vulnerable = false;
            
            foreach(Transform child in characterVisualsTr)
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

            vulnerable = true;
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
                characterFaces[0].SetActive(true);
                characterFaces[1].SetActive(false);
                characterFaces[2].SetActive(false);
            }

            if(degrees > 145 || degrees < -145)
            {
                isFacingLeft = true;
                characterFaces[0].SetActive(false);
                characterFaces[1].SetActive(false);
                characterFaces[2].SetActive(true);
            }

            if (degrees > -145 && degrees < -125)
            {
                isFacingLeft = true;
                characterFaces[0].SetActive(true);
                characterFaces[1].SetActive(false);
                characterFaces[2].SetActive(false);
            }

            if (degrees > -125 && degrees < -55)
            {
                isFacingLeft = true;
                characterFaces[0].SetActive(true);
                characterFaces[1].SetActive(false);
                characterFaces[2].SetActive(false);
            }

            if (degrees > -55 && degrees < 35)
            {
                isFacingLeft = false;
                characterFaces[0].SetActive(false);
                characterFaces[1].SetActive(true);
                characterFaces[2].SetActive(false);
            }
        }
    }
}
