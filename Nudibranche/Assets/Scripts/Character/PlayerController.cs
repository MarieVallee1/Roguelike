using System;
using System.Collections;
using System.Collections.Generic;
using Character.Skills;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        public static PlayerController Instance;
        
        [Header("Character System Related")]
        [HideInInspector]public Rigidbody2D _rb;
        private Transform _tr;
        public PlayerInputActions characterInputs;
        public CharacterData characterData;
        public SkillsDetails skills;
        [SerializeField] private GameObject cursor;
        [SerializeField] private Transform startRoomTp;
        [SerializeField] private Transform dashPosition;
        [SerializeField] private Transform feetPosition;
        [SerializeField] private SpriteRenderer tpMat;
        [SerializeField] private Material[] charaMat;
        
        [Header("Book Related")]
        [SerializeField] private GameObject book;
        [SerializeField] private Transform bookPos;
        [SerializeField] private Animator bookAnim;
        
        [Header("Parry Related")]
        [SerializeField] private Transform parryCooldown;
        [SerializeField] private ParticleSystem parryFeedback;
        [SerializeField] private ParticleSystem parryHitFeedback;
        [SerializeField] private VisualEffect parryActivationVFX;
        [SerializeField] private ParryRepulsion parryRepulsion;
        [SerializeField] private GameObject buffVFX;

        [Header("Character Visuals Related")]
        [SerializeField] private GameObject characterVisualsTr;
        [SerializeField] private GameObject dashVFX;
        [SerializeField] private GameObject[] characterFaces;
        [SerializeField] private Animator[] animator;
        public List<SpriteRenderer> visuals;


        public float shaderDissolveValue = 2;
        public float dissolveDuration = 0.5f;
        public float ressolveDuration = 0.5f;
        
        
        #region Variables
        
        public Vector2 movementDirection;
        public Vector2 aim;
        [HideInInspector] public Vector2 characterPos;
        [HideInInspector] public Vector2 shootDir;
        
        [HideInInspector] public float nextTimeShoot;
        [HideInInspector] public float nextTimeDash;
        
        [Header("Number of Projectile Left")]
        public int remainingProjectile;
        
        public int skillIndex;

        private float _nextTimeParry;
        private float _parryLifeTime;
        public float skillCountdown;
        private float _blastCooldown;
        public float reloadCountdown = 2f;
        
        [Header("Stats")]
        public float skillCooldown;
        public int health;
        public float dashCooldown;
        public float speed;
        public float damage;
        public float fireRate;
        public float projectileSize;
        

        [Header("State")]
        public bool gamepadOn;
        [Space]
        public bool vulnerable;
        public bool stopPlayerMovement;
        public bool onShoot;
        public bool onParry;
        public bool onBuff;
        public bool onSkillUse;
        public bool isDead;
        [Space]
        public bool movementPressed;
        public bool isMovingUp;
        public bool isMovingDown;
        public bool isFacingLeft;
        private static readonly int Dissolve = Shader.PropertyToID("Dissolve");

        #endregion

        //Audio
        [SerializeField] private AudioSource audioSource;
        private bool _canDash = true;
        
        //FixParryBuff
        private int _parryCounter;
        
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
            dashCooldown = characterData.dashCooldown;
            damage = characterData.usedProjectile[0].damage;
            fireRate = characterData.usedProjectile[0].fireRate;
            projectileSize = characterData.usedProjectile[0].projectileSize;
            vulnerable = true;
            remainingProjectile = characterData.usedProjectile[characterData.projectileIndex].blastLenght;
            _blastCooldown = characterData.usedProjectile[characterData.projectileIndex].blastCooldown;
            tpMat.enabled = false;
            isDead = false;

            for (int i = 0; i < visuals.Count; i++)
            {
                visuals[i].GetComponent<Renderer>().material = charaMat[0];
            }

            //Set the skill to null
            //skillIndex = 0;

            nextTimeDash = dashCooldown;
        }

        
        private void OnEnable()
        {
            characterInputs.UI.Disable();
            characterInputs.Character.Enable();

            characterInputs.Character.Movement.performed += ctx =>
            {
                movementDirection = ctx.ReadValue<Vector2>();
                movementPressed = movementDirection.x != 0 || movementDirection.y != 0;
                isMovingUp = movementDirection.y > 0.7;
                isMovingDown = movementDirection.y < -0.7;
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
                gamepadOn = true;
                //Handles the direction of the projectile if shot with the gamepad
            };
            
            characterInputs.Character.AimMouse.performed += ctx =>
            {
                gamepadOn = false;
                
                //Enable the cursor when shooting with the mouse
                cursor.SetActive(true);
            };
            
            characterInputs.Character.Parry.performed += ctx =>
            {
                if (ParryCooldown() && !onParry)
                {
                    AudioList.Instance.PlayOneShot(AudioList.Instance.playerParry,AudioList.Instance.playerParryVolume);
                    
                    //Plays a VFX when you parry
                    parryActivationVFX.Play();
                    
                    onParry = true;
                }
            };

            characterInputs.Character.Teleportation.performed += ctx => StartCoroutine(HandleTeleportation());
        }
        private void OnDisable()
        {
            characterInputs.Disable();
            movementPressed = false;
        }
        
        
        private void Update()
        {
            Debug.DrawRay(characterPos,aim.normalized*Vector3.Distance(characterPos, dashPosition.position),Color.red);
            
            
            HandleParry();
            HandleMouseLook();
            RestrictMousePos();
            Flip();
            HandleSpriteRotation();
            ParryCooldown();
            AttackCooldown();
            if (!_canDash) DashCooldown();
            BlastReload();
            HandleSkillUse();
            DashExtra();
            HandleBuffFeedback();

            if (health <= 0 && !GameManager.instance.cheatDeath && !isDead)
            {
                isDead = true;
                StartCoroutine(PlayerDeath());
            }
            
            if (remainingProjectile <= 0)
            {
                _blastCooldown -= Time.deltaTime;
                BlastCooldown(_blastCooldown);
            }
            
            if (_canDash)
            {
                if (characterInputs.Character.Dash.triggered && CanDash())
                {
                    _canDash = false;
                    StartCoroutine(HandleDashUse());
                }
            }

            characterPos = _tr.position;
            skillCountdown += Time.deltaTime;

            if (characterInputs.Character.AimGamepad.triggered)
            {
                characterInputs.Character.AimGamepad.performed += ctx =>
                {
                    aim = ctx.ReadValue<Vector2>();
                };
            }
        }
        private void FixedUpdate()
        {
            //Shoots the projectile
            if (onShoot && remainingProjectile > 0) Shoot();
            else
            {
                bookAnim.SetBool("isShooting", false);
            }
            
            if(!stopPlayerMovement)HandleMovement();
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
                    audioSource.PlayOneShot(AudioList.Instance.playerHit,AudioList.Instance.playerHitVolume);
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
            //PostProcessing.Instance._chromaticAberration.intensity.value = 1;
            CinemachineShake.instance.ShakeCamera(1f,0.2f);
            PostProcessing.Instance.gotHit = true;
            vulnerable = false;
            
            // for (int i = 0; i < 3; i++)
            // {
            //     for (int y = 0; y < visuals.Count; y++)
            //     {
            //         Color tmp = visuals[y].color;
            //         tmp.a = 0;
            //         visuals[y].color = tmp;
            //     }
            //    
            //     yield return new WaitForSeconds(0.2f);
            //     
            //     for (int t = 0; t < visuals.Count; t++)
            //     {
            //         Color tmp = visuals[t].color;
            //         tmp.a = 1;
            //         visuals[t].color = tmp;
            //     }
            //     
            //     yield return new WaitForSeconds(0.2f);
            // }
            
            for (int i = 0; i < 1; i++)
            {
            
                for (int y = 0; y < visuals.Count; y++)
                {
                    visuals[y].GetComponent<Renderer>().material = charaMat[1];
                }
               
                yield return new WaitForSeconds(0.2f);
                
                for (int k = 0; k < visuals.Count; k++)
                {
                    visuals[k].GetComponent<Renderer>().material = charaMat[0];
                }
                
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(0.5f);
            
            vulnerable = true;
            PostProcessing.Instance._chromaticAberration.intensity.value = 0;
        }
        private IEnumerator PlayerDeath()
        {
            print("I'm Dead");
            GameManager.instance.currentRoom.ResetRoom();
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            vulnerable = false;

            for (int i = 0; i < animator.Length; i++)
            {
                animator[i].SetBool("isDead", true);
            }
            yield return new WaitForSeconds(0.5f);
            UIManager.instance.BlackScreenFadeOut();
            yield return new WaitForSeconds(1f);
            GameManager.instance.TpHub();
            UIManager.instance.OpenDeathScreen();
            ScoreManager.instance.UpdateAllScore();
    
        }
        
        
        private void HandleMovement()
        {
            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, characterData.maxSpeed);
            
            //Moves the character
            if (movementPressed)
            {
                //If the player does a successful parry its movement speed is increased
                if(!onBuff && !onParry)speed = characterData.speed;
                else if (onParry) speed = characterData.speed / 2f;
                else speed = characterData.speedBuff;
                
                //Moves the character
                _rb.AddForce(movementDirection * speed,ForceMode2D.Impulse);
                
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
        private void HandleBuffFeedback()
        {
            if(onBuff) buffVFX.SetActive(true);
            else buffVFX.SetActive(false);
        }
        private void HandleMouseLook()
        {
            if (!gamepadOn)
            {
                aim = cursor.transform.position - transform.position;
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
                usedProjectile.transform.position = bookPos.transform.position;

                //usedProjectile.transform.eulerAngles = (-BookPosition.Instance.transform.eulerAngles);
                
                usedProjectile.SetActive(true);
                shootDir = aim.normalized;
            
                //Handles the movement of the projectile
                usedProjectile.GetComponent<Rigidbody2D>().velocity = shootDir * characterData.usedProjectile[characterData.projectileIndex].projectileSpeed;

                //Reset the fire rate
                nextTimeShoot = Time.time + fireRate;

                //Decreases the amount of projectile in a blast
                remainingProjectile -= 1;

                //Reset the reload countdown 
                reloadCountdown = 2f;
            }
        }
        private IEnumerator Parry()
        {
            _parryCounter++;
            var currentParry = _parryCounter;
            AudioList.Instance.PlayOneShot(AudioList.Instance.enemyHit,1);
            parryHitFeedback.Play();
            
            vulnerable = false;
            //Detect enemies near
            parryRepulsion.enabled = true;
            yield return new WaitForSeconds(0.2f);
            //Stop detecting enemies
            parryRepulsion.enabled = false;

            //Play VFX
            if(parryFeedback.isStopped) parryFeedback.Play();

            //Activate Buff
            onBuff = true;
            yield return new WaitForSeconds(2f);
            vulnerable = true;
            yield return new WaitForSeconds(characterData.buffDuration);
            if (currentParry == _parryCounter)
            {
                onBuff = false;
                _parryCounter = 0;
            }
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
                
                //Slow the character speed
                speed *= 0.75f;
                
                
                //Decrease the duration of the parry time through time
                _parryLifeTime -= Time.deltaTime;
            }
            
            //End of parry
            if (_parryLifeTime < 0f)
            {
                //Reset the position of the parry cooldown feedback
                // parryCooldown.localScale = new Vector3(1, 1, 1);
                //
                // //Scale down the parry cooldown feedback
                // parryCooldown.DOScale(new Vector3(0, 0, 1),characterData.parryCooldown);
                
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
            if(characterInputs.Character.Skill.triggered && skillCountdown > skillCooldown)
            {
                
                switch (skillIndex)
                {
                    case 0:
                        StartCoroutine(skills.SwordSlash());
                        break;
                    case 1:
                        skills.WrongTrack(characterPos);
                        Debug.Log("skill");
                        break;
                    case 2:
                        StartCoroutine(skills.CardLaser(bookPos.position, aim));
                        break;
                }
            }
        }
        private IEnumerator HandleDashUse()
        {
            //Audio
            audioSource.PlayOneShot(AudioList.Instance.playerDash);
            
            //Cooldown
            nextTimeDash = 0;
            
            //Rigged Sprite deactivation
            characterVisualsTr.SetActive(false);
            
            //VFX Previous position
            Instantiate(dashVFX, characterPos, quaternion.identity);
            
            //Post Process feedback
            PostProcessing.Instance._lensDistortion.intensity.value = 0f;
            PostProcessing.Instance.dashing = true;
            PostProcessing.Instance._lensDistortion.intensity.value = -0.5f;

            Debug.Log("I Dash");

            //Dissolve Material activation
            tpMat.enabled = true;
            ressolveDuration = 0;
            
            yield return new WaitForSeconds(0.1f);
            //Teleportation
            _tr.position = dashPosition.position;

            yield return new WaitForSeconds(0.4f);
            
            //Rigged Sprite activation
            characterVisualsTr.SetActive(true);
            
            //Dissolve Material deactivation
            tpMat.enabled = false;
        }
        

        private bool CanDash()
        {
            RaycastHit2D hit = Physics2D.Raycast(feetPosition.position, movementDirection, Vector3.Distance(feetPosition.position, dashPosition.position),layerMask:LayerMask.GetMask("Mur"));
            if (hit)
            {
                Debug.Log(hit.collider.name);
                return false;
            }
            
            return true;
        }
        private void DashExtra()
        {
            if (dissolveDuration < 1)
            {
                shaderDissolveValue = Mathf.Lerp(2, -2, dissolveDuration);
                dissolveDuration += 1.5f * Time.deltaTime;
            }
            else dissolveDuration = 1;

            if (ressolveDuration < 1)
            {
                shaderDissolveValue = Mathf.Lerp(-2, 2, ressolveDuration);
                ressolveDuration += 1.4f * Time.deltaTime;
            }
            else ressolveDuration = 1;
            
            tpMat.material.SetFloat("_Dissolve", shaderDissolveValue);
        }
        private IEnumerator HandleTeleportation()
        {
            UIManager.instance.BlackScreenFadeIn();
            yield return new WaitForSeconds(1f);
            _tr.position = startRoomTp.position;
            GameManager.instance.ReloadStart();
            yield return new WaitForSeconds(1f);
            UIManager.instance.BlackScreenFadeOut();
        }
        

        private bool AttackCooldown()
        {
            //Handles the cooldown of the basic attack
            if(Time.time > nextTimeShoot) return true;
            return false;
        }
        private void DashCooldown()
        {
            //Handles the cooldown of the basic attack
            if (nextTimeDash > dashCooldown)
            {
                _canDash = true;
                var sound = AudioList.Instance;
                sound.PlayOneShot(sound.uiClick,0.08f);
            }
            else nextTimeDash += Time.deltaTime;
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
            reloadCountdown -= Time.deltaTime;
            
            //Reset the blast if the player is not shooting for a certain time   
            if(reloadCountdown <= 0) remainingProjectile = characterData.usedProjectile[characterData.projectileIndex].blastLenght;
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
            stopPlayerMovement = true;
            movementPressed = false;
        }
        public void UnfreezeCharacter()
        {
            stopPlayerMovement = false;
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
