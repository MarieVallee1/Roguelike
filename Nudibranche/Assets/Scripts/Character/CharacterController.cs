using System.Threading;
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

        #region Variables
        private Vector2 _direction;
        [HideInInspector] public Vector2 aim;
        [HideInInspector] public float nextTimeCast; 
        private float _nextTimeParry;
        private float _parryLifeTime;
        [Header("State")]
        public bool isShootingGamepad;
        public bool isShootingMouse;
        public bool isParrying;
        //private int _isRunningHash;
        //private bool _movementPressed;
        #endregion

        private void Awake()
        {
            _characterInputs = new PlayerInputActions();
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _tr = GetComponent<Transform>();
            //_animator = GetComponent<Animator>();
            _spriteRen = GetComponent<SpriteRenderer>();

            //_isRunningHash = Animator.StringToHash("isRunning");

            _parryLifeTime = characterData.parryTime;
        }

        private void Update()
        {
            HandleMovement();
            AttackCooldown();
            HandleParry();
            ParryCooldown();
            
            //Shoot the projectile
            if(isShootingGamepad || isShootingMouse) usedProjectile.CharacterShooting(this, mousePos.transform.position);
            
            //Handle the direction of the projectile if shot with the mouse
            if(isShootingMouse) MousePosition();
        }

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
                //Handle the direction of the projectile if shot with the gamepad
                if(isShootingGamepad) aim = ctx.ReadValue<Vector2>();
            };
            
            //Allow to detect which controller is used 
            _characterInputs.Character.ShootGamepad.performed += ctx => isShootingGamepad = true;
            _characterInputs.Character.ShootGamepad.canceled += ctx => isShootingGamepad = false;
            _characterInputs.Character.ShootMouse.performed += ctx => isShootingMouse = true;
            _characterInputs.Character.ShootMouse.canceled += ctx => isShootingMouse = false;
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
            //Move the character
            _rb.velocity = _direction * characterData.speed;

            //Flip the sprite when facing left
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
            if (isParrying) _parryLifeTime -= Time.deltaTime;
            if (_parryLifeTime < 0f)
            {
                _characterInputs.Character.Parry.Enable();
                _nextTimeParry = characterData.parryCooldown;
                isParrying = false;
            }
        }

        public bool AttackCooldown()
        {
            if(Time.time > nextTimeCast) return true;
            return false;
        }
        public bool ParryCooldown()
        {
            if(Time.time > _nextTimeParry) return true;
            return false;
        }

        public void MousePosition()
        {
            aim = mousePos.transform.position - _tr.position;
        } 
        
    }
}
