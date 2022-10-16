using System.Collections;
using DG.Tweening;
using Projectiles;
using Unity.VisualScripting;
using UnityEngine;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputActions _characterInputs;

        private Rigidbody2D _rb;
        private Transform _tr;
        private SpriteRenderer _spriteRen;
        public static PlayerController instance;
        
        //private Animator _animator;

        [Header("References")]
        [SerializeField] private CharacterData characterData;
        [SerializeField] private Projectile usedProjectile;
        [SerializeField] private GameObject bookPos;
        [SerializeField] private GameObject mouseCursor;
        [SerializeField] private Transform parryCooldown;

        #region Variables
        private Vector2 _direction;
        private Vector2 _mouseAim;
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
        #endregion

        //private int _isRunningHash;
        [SerializeField] private bool movementPressed;

        private void Awake()
        {
            if (instance != null && instance != this)
                Destroy(gameObject);
 
            instance = this;
            
            _characterInputs = new PlayerInputActions();
            
            _rb = GetComponent<Rigidbody2D>();
            _tr = GetComponent<Transform>();
            _spriteRen = GetComponent<SpriteRenderer>();

            _parryLifeTime = characterData.parryTime;
            _rb.drag = characterData.drag;
            _health = characterData.health;
            canGethit = true;
            
            //_animator = GetComponent<Animator>();
            //_isRunningHash = Animator.StringToHash("isRunning");
        }
   
        private void Update()
        {
            characterPos = _tr.position;
            RestrictMousePos();
        }
        private void FixedUpdate()
        {
            //Shoots the projectile
            if(isShooting) usedProjectile.CharacterShooting(this, bookPos.transform.position);
            
            HandleMovement();
            speedDebug = _rb.velocity.magnitude;
            
            AttackCooldown();
            
            HandleParry();
            ParryCooldown();
        }
        
        
        private void OnEnable()
        {
            _characterInputs.Enable();

            _characterInputs.Character.Movement.performed += ctx =>
            {
                _direction = ctx.ReadValue<Vector2>();
                movementPressed = _direction.x != 0 || _direction.y != 0;
            };
            
            //Allows to detect which controller is used 
            _characterInputs.Character.ShootGamepad.performed += ctx =>
            {
                gamepadOn = true;
                isShooting = true;
            };
            _characterInputs.Character.ShootGamepad.canceled += ctx => isShooting = false;
            _characterInputs.Character.ShootMouse.performed += ctx =>
            {
                gamepadOn = false;
                isShooting = true;
            };
            _characterInputs.Character.ShootMouse.canceled += ctx => isShooting = false;
                _characterInputs.Character.AimGamepad.performed += ctx =>
            {
                //Disable the cursor when aiming with the gamepad
                mouseCursor.SetActive(false);
                //Handles the direction of the projectile if shot with the gamepad
                aim = ctx.ReadValue<Vector2>();
            };
            
            _characterInputs.Character.AimMouse.performed += ctx =>
            {
                //Enable the cursor when shooting with the mouse
                mouseCursor.SetActive(true);
                _mouseAim = ctx.ReadValue<Vector2>();
                if(!gamepadOn)aim = new Vector2(_mouseAim.x - GameManager.instance.screenWidth / 2, _mouseAim.y - GameManager.instance.screenHeight / 2) + characterPos;
            };
            
            _characterInputs.Character.Parry.performed += ctx =>
            {
                if (ParryCooldown() && !isParrying) isParrying = true;
            };
        }
        private void OnDisable()
        {
            _characterInputs.Disable();
        }
        

        private void HandleMovement()
        {
            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, characterData.maxSpeed);
            
            //Moves the character
            if (movementPressed)
            {
                _rb.AddForce(_direction * characterData.speed,ForceMode2D.Impulse);
            }
            
            //Flips the sprite when facing left
            if (_direction.x > 0) _spriteRen.flipX = true;
            if (_direction.x < 0) _spriteRen.flipX = false;


            #region Animation

            // bool isRunning = _animator.GetBool(_isRunningHash);
            //
            // if (_movementPressed) _animator.SetBool(_isRunningHash, true);
            // else _animator.SetBool(_isRunningHash, false);

            #endregion
  
        }
        private void HandleParry()
        {
            if (isParrying)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                _spriteRen.color = Color.red;
                _rb.velocity = Vector2.zero;
                _parryLifeTime -= Time.deltaTime;
                DisableInputs();
            }
            
            //End of parry
            if (_parryLifeTime < 0f)
            {
                parryCooldown.localScale = new Vector3(1, 1, 1);
                
                _rb.constraints = RigidbodyConstraints2D.None;
                _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                _spriteRen.color = Color.white;
                
                parryCooldown.DOScale(new Vector3(0, 0, 1),characterData.parryCooldown);
                _nextTimeParry = Time.time + characterData.parryCooldown;
                _parryLifeTime = characterData.parryTime;
                isParrying = false;

                movementPressed = false;
                
                EnableInputs();
            }
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
        private IEnumerator Parry()
        {
            isBuffed = true;
            yield return new WaitForSeconds(characterData.buffDuration);
            isBuffed = false;
        }
        
        private void DisableInputs()
        {
            _characterInputs.Character.Disable();
        }
        private void EnableInputs()
        {
            _characterInputs.Character.Enable();
        }
        
        private void RestrictMousePos()
        {
            Display.RelativeMouseAt(characterPos);
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
                
                StartCoroutine(Invulnerability());
                
                _health -= damage;
                
                //Set the UI to the right amount of hearts
                Health.instance.SetHealth(_health);
                
                print("I got hit !");
            }
        }

        private IEnumerator Invulnerability()
        {
            canGethit = false;
            
            _spriteRen.DOFade(0, 0.1f);
            yield return new WaitForSeconds(0.1f);
            _spriteRen.DOFade(1, 0.1f); 
            yield return new WaitForSeconds(0.1f);
            _spriteRen.DOFade(0, 0.1f);
            yield return new WaitForSeconds(0.1f);
            _spriteRen.DOFade(1, 0.1f); 

            canGethit = true;
        }
    }
}
