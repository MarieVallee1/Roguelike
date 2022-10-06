using DG.Tweening;
using Projectiles;
using UnityEngine;

namespace Character
{
    public class CharacterController : MonoBehaviour
    {
        private PlayerInputActions _characterInputs;

        private Rigidbody2D _rb;
        private Transform _tr;
        private SpriteRenderer _spriteRen;
        
        //private Animator _animator;

        [Header("References")]
        [SerializeField] private CharacterData characterData;
        [SerializeField] private Projectile usedProjectile;
        [SerializeField] private GameObject mousePos;
        [SerializeField] private Transform parryCooldown;

        #region Variables
        private Vector2 _direction;
        [HideInInspector] public Vector2 aim;
        [HideInInspector] public Vector2 characterPos;
        public GameObject debug;
        [HideInInspector] public float nextTimeCast; 
        private float _nextTimeParry;
        private float _parryLifeTime;
        [Header("State")]
        public bool isShootingGamepad;
        public bool isShootingMouse;
        public bool isParrying;
        #endregion

        //private int _isRunningHash;
        //private bool _movementPressed;

        private void Awake()
        {
            _characterInputs = new PlayerInputActions();
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _tr = GetComponent<Transform>();
            _spriteRen = GetComponent<SpriteRenderer>();

            _parryLifeTime = characterData.parryTime;
            
            //_animator = GetComponent<Animator>();
            //_isRunningHash = Animator.StringToHash("isRunning");
        }

        private void Update()
        {
            debug.transform.position = aim;
            characterPos = _tr.position;
            
            HandleMovement();
            
            AttackCooldown();
            
            HandleParry();
            ParryCooldown();
            
            
            //Shoots the projectile
            if(isShootingGamepad || isShootingMouse) usedProjectile.CharacterShooting(this, mousePos.transform.position);
        }

        //Manages the inputs
        private void OnEnable()
        {
            _characterInputs.Enable();

            _characterInputs.Character.Movement.performed += ctx =>
            {
                _direction = ctx.ReadValue<Vector2>();
                //_movementPressed = _direction.x != 0 || _direction.y != 0;
            };


            _characterInputs.Character.AimGamepad.performed += ctx =>
            {
                //Handles the direction of the projectile if shot with the gamepad
                if(isShootingGamepad) aim = ctx.ReadValue<Vector2>();
            };
            
            //Allows to detect which controller is used 
            _characterInputs.Character.ShootGamepad.performed += ctx => isShootingGamepad = true;
            _characterInputs.Character.ShootGamepad.canceled += ctx => isShootingGamepad = false;
            _characterInputs.Character.ShootMouse.performed += ctx => isShootingMouse = true;
            _characterInputs.Character.ShootMouse.canceled += ctx => isShootingMouse = false;
            _characterInputs.Character.AimMouse.performed += ctx =>
            {
                aim = ctx.ReadValue<Vector2>();
            };
            _characterInputs.Character.Parry.performed += ctx =>
            {
                if (ParryCooldown() && !isParrying)
                {
                    Debug.Log(1);
                    isParrying = true;
                }
            };
            
        }
        
        private void OnDisable()
        {
            _characterInputs.Disable();
        }

        private void HandleMovement()
        {
            //Moves the character
            _rb.velocity = _direction * characterData.speed;

            //Flips the sprite when facing left
            if (_direction.x < 0) _spriteRen.flipX = true;
            if (_direction.x > 0) _spriteRen.flipX = false;


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
                _spriteRen.color = Color.white;
                parryCooldown.DOScale(new Vector3(0, 0, 1),characterData.parryCooldown);
                
                EnableInputs();
                _nextTimeParry = Time.time + characterData.parryCooldown;
                _parryLifeTime = characterData.parryTime;
                isParrying = false;
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
            Mathf.Clamp(Input.mousePosition.x, aim.x  , aim.x);
            Mathf.Clamp(Input.mousePosition.y,aim.x, aim.y);
        }
    }
}
