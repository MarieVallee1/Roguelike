using System;
using System.Collections;
using Character.Skills;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
        public SkillsDetails skills;
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
        [SerializeField]
        public string currentSkill;
        [HideInInspector] public int skillIndex;

        private float _nextTimeParry;
        private float _parryLifeTime;
        private float _skillCountdown;
        private float _blastCooldown;
        private float _reloadCountdown;
        
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
            damage = characterData.usedProjectile[0].damage;
            fireRate = characterData.usedProjectile[0].fireRate;
            projectileSize = characterData.usedProjectile[0].projectileSize;
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
            BlastReload();
            
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
            else
            {
                bookAnim.SetBool("isShooting", false);
            }

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
                    //Debugs Death
                    if (health <= 0)
                    {
                        print("I'm Dead");
                        health = characterData.health;
                        health = 0;
                    }
                
                    //Make the character invulnerable for a certain time
                    StartCoroutine(InvulnerabilityFrame(characterData.invulnerabilityDuration));
                    
                    //Subtract the damage received to the current health
                    health -= damage;
                
                    //Set the UI to the right amount of hearts
                    Health.instance.SetHealth(health);
                    
                    //Debugs the remaining health
                    print("Remaining health :" + health);
                }

            }
        }      
        private IEnumerator InvulnerabilityFrame(float invulnerabilityDuration)
        {
            vulnerable = false;
            //Invulnerability duration
            yield return new WaitForSeconds(invulnerabilityDuration);
            vulnerable = true;
        }
        
        
        private void HandleMovement()
        {
            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, characterData.maxSpeed);
            
            //Moves the character
            if (movementPressed)
            {
                //If the player does a successful parry its movement speed is increased
                if(!onBuff)speed = characterData.speed;
                else speed = characterData.speedBuff;
                
                //Moves the character
                _rb.AddForce(_movementDirection * speed,ForceMode2D.Impulse);
                
                //Handle the running animation of all the faces
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
        private void Shoot()
        {
            //Get the projectile type we want to shoot
            GameObject usedProjectile = PoolingSystem.instance.GetObject(characterData.usedProjectile[characterData.projectileIndex].usedProjectileName);

            if (usedProjectile != null && AttackCooldown())
            { 
                //Camera Shake
                //CinemachineShake.instance.ShakeCamera(0.3f,0.1f);
                
                //Handles the animation of the book when you shoot
                bookAnim.SetBool("isShooting", true);
                
                //Placement & activation of the projectile
                usedProjectile.transform.position = book.transform.position;
                usedProjectile.SetActive(true);
                shootDir = aim.normalized;
            
                //Handles the movement of the projectile
                usedProjectile.GetComponent<Rigidbody2D>().velocity = shootDir * characterData.usedProjectile[characterData.projectileIndex].projectileSpeed;

                //Reset the fire rate
                nextTimeShoot = Time.time + fireRate;

                //Decreases the amount of projectile in a blast
                remainingProjectile -= 1;

                //Reset the reload countdown 
                _reloadCountdown = 2f;
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
        private void HandleParry()
        {
            if (onParry)
            {
                //Handle the parry animation for all faces
                for (int i = 0; i < animator.Length; i++)
                {
                    animator[i].SetBool("isParrying", true);
                }

                //Freezes the character
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                _rb.velocity = Vector2.zero;
                
                //Decrease the duration of the parry time through time
                _parryLifeTime -= Time.deltaTime;
            }
            
            //End of parry
            if (_parryLifeTime < 0f)
            {
                //Reset the position of the parry cooldown feedback
                parryCooldown.localScale = new Vector3(1, 1, 1);

                //Unfreezes the character
                var constraints = _rb.constraints;
                constraints = RigidbodyConstraints2D.None;
                constraints = RigidbodyConstraints2D.FreezeRotation;
                _rb.constraints = constraints;

                //Scale down the parry cooldown feedback
                parryCooldown.DOScale(new Vector3(0, 0, 1),characterData.parryCooldown);
                
                //Reset the countdown between 2 parry
                _nextTimeParry = Time.time + characterData.parryCooldown;
                
                //Reset the countdown of the parry life time
                _parryLifeTime = characterData.parryTime;
                
                onParry = false;
                
                //Reset the position of the parry cooldown feedback
                for (int i = 0; i < animator.Length; i++)
                {
                    animator[i].SetBool("isParrying", false);
                }
            }
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
                        currentSkill = "Sword Slash";
                    }
                        break;
                    case 1:
                    {
                        skills.WrongTrack(characterPos);
                        currentSkill = "Wrong Track";
                    }
                        break;
                    case 2:
                    {
                        StartCoroutine(skills.CardLaser(bookPos.position, aim));
                        currentSkill = "Card Laser";
                    }
                        break;
                }
            };
        }

        
        private bool AttackCooldown()
        {
            //Handles the cooldown of the basic attack
            if(Time.time > nextTimeShoot) return true;
            return false;
        }
        private void BlastCooldown(float nextTimeBlast)
        {
            //Handles the amount of projectile in one blast 
            if (nextTimeBlast <= 0)
            {
                remainingProjectile = characterData.usedProjectile[characterData.projectileIndex].blastLenght;
                _blastCooldown = characterData.usedProjectile[characterData.projectileIndex].blastCooldown;
            }
        }

        private void BlastReload()
        {
            _reloadCountdown -= Time.deltaTime;
            
            //Reset the blast if the player is not shooting for a certain time   
            if(_reloadCountdown <= 0) remainingProjectile = characterData.usedProjectile[characterData.projectileIndex].blastLenght;
        }
        private bool ParryCooldown()
        {
            //Handles the cooldown of the parry
            if(Time.time > _nextTimeParry) return true;
            return false;
        }
        
        
        private void Flip()
        {
            //Flips the sprite considering the direction;
            characterVisualsTr.transform.localScale = !isFacingLeft ? new Vector3(-1, 1, 1) : new Vector3(1,1,1);
        }
        public void DisableInputs()
        {
            characterInputs.Character.Disable();
            //Enables the UI inputs
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
            //The mouse can't go out of the screen
            Display.RelativeMouseAt(characterPos);
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
