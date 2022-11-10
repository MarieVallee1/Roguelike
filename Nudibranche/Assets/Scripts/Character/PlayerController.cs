using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Projectiles;
using UnityEngine;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerInputActions characterInputs;
        private Rigidbody2D _rb;
        private Transform _tr;
        public static PlayerController instance;
        
        private Animator _animator;

        [Header("References")]
        [SerializeField] private CharacterData characterData;
        [SerializeField] private Projectile usedProjectile;
        [SerializeField] private GameObject book;
        [SerializeField] private GameObject cursor;
        [SerializeField] private Transform parryCooldown;
        [SerializeField] private Transform visualTr;
        [SerializeField] private GameObject visuals;
        
        public List<Skills> skills;
        public Skills currentSkill;

        #region Variables
        private Vector2 _direction;
        public Vector2 mouseAim;
        [HideInInspector] public Vector2 aim;
        [HideInInspector] public Vector2 characterPos;

        [HideInInspector] public float nextTimeCast;
        
        private float _nextTimeParry;
        private float _parryLifeTime;
        [Space]
        [SerializeField] private float speedDebug;
        
        private int _health;

        [Header("State")]
        public bool canGethit;
        public bool gamepadOn;
        public bool isShooting;
        public bool isParrying;
        public bool isBuffed;
        public bool isMovingUp;
        public bool isMovingDown;
        public bool isFacingLeft;
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

            _parryLifeTime = characterData.parryTime;
            _rb.drag = characterData.drag;
            _health = characterData.health;
            canGethit = true;
            
            _animator = GetComponentInChildren<Animator>();
            _isRunningHash = Animator.StringToHash("isRunning");
            
        }
        
        
        private void Update()
        {
            characterPos = _tr.position;
            RestrictMousePos();
            Flip();
        }
        private void FixedUpdate()
        {
            //Shoots the projectile
            if(isShooting) usedProjectile.CharacterShooting(this, book.transform.position);
            
            HandleMovement();
            speedDebug = _rb.velocity.magnitude;
            
            AttackCooldown();
            
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
                StartCoroutine(Parry());
                print("I'm Parrying !");
            }
            else
            {
                
                //Debugs Death :D 
                if (_health <= 0)
                {
                    Debug.Log("You're dead");
                    _health = characterData.health;
                }
                
                StartCoroutine(InvulnerabilityFrame());
                
                _health -= damage;
                
                //Set the UI to the right amount of hearts
                Health.instance.SetHealth(_health);
                
                print("I got hit !");
            }
        }

        
        private void HandleMovement()
        {
            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, characterData.maxSpeed);
            
            //Moves the character
            if (movementPressed)
            {
                _rb.AddForce(_direction * characterData.speed,ForceMode2D.Impulse);
            }
            
            // //Flips the sprite when facing left
            if (_direction.x > 0) isFacingLeft = false;
            if (_direction.x < 0) isFacingLeft = true;


            #region Animation

            // if (isMovingUp)
            // {
            //     //Back
            //     _spriteRen.sprite = charaDirSprites[2];
            // }else if (isMovingDown)
            // {
            //     //Front
            //     _spriteRen.sprite = charaDirSprites[0];
            // }
            // else
            // {
            //     // 3/4
            //     _spriteRen.sprite = charaDirSprites[1];
            // }
            
            
            bool isRunning = _animator.GetBool(_isRunningHash);
            
            if (movementPressed) _animator.SetBool(_isRunningHash, true);
            else _animator.SetBool(_isRunningHash, false);

            #endregion
  
        }
        private void HandleParry()
        {
            if (isParrying)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                
                foreach(Transform child in visualTr)
                {
                    SpriteRenderer ren = child.GetComponent<SpriteRenderer>();
                    ren.color = Color.red;
                }
                
                _rb.velocity = Vector2.zero;
                _parryLifeTime -= Time.deltaTime;
                DisableInputs();
            }
            
            //End of parry
            if (_parryLifeTime < 0f)
            {
                parryCooldown.localScale = new Vector3(1, 1, 1);

                var constraints = _rb.constraints;
                constraints = RigidbodyConstraints2D.None;
                constraints = RigidbodyConstraints2D.FreezeRotation;
                _rb.constraints = constraints;
                
                foreach(Transform child in visualTr)
                {
                    SpriteRenderer ren = child.GetComponent<SpriteRenderer>();
                    ren.color = Color.white;
                }

                parryCooldown.DOScale(new Vector3(0, 0, 1),characterData.parryCooldown);
                _nextTimeParry = Time.time + characterData.parryCooldown;
                _parryLifeTime = characterData.parryTime;
                isParrying = false;

                movementPressed = false;
                
                EnableInputs();
            }
        }
        private IEnumerator Parry()
        {
            isBuffed = true;
            yield return new WaitForSeconds(characterData.buffDuration);
            isBuffed = false;
        }
        public bool AttackCooldown()
        {
            if(Time.time > nextTimeCast) return true;
            return false;
        }
        private bool ParryCooldown()
        {
            if(Time.time > _nextTimeParry) return true;
            return false;
        }
 
        
        public void FreezeCharacter()
        {
            DisableInputs();
            _rb.velocity = Vector2.zero;
        }
        private void Flip()
        {
            visuals.transform.localScale = !isFacingLeft ? new Vector3(-1, 1, 1) : new Vector3(1,1,1);
        }
        public void DisableInputs()
        {
            characterInputs.Character.Disable();
            characterInputs.UI.Enable();
            
            //Stop the player from running (anim)
            _animator.SetBool(_isRunningHash, false);
        }
        public void EnableInputs()
        {
            characterInputs.Character.Enable();
            characterInputs.UI.Disable();
        }
        private void RestrictMousePos()
        {
            Display.RelativeMouseAt(characterPos);
        }
        private IEnumerator InvulnerabilityFrame()
        {
            canGethit = false;
            
            foreach(Transform child in visualTr)
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
    }
}
