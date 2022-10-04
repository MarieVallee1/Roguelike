using Projectiles;
using UnityEngine;

namespace Character
{
    public class CharacterController : MonoBehaviour
    {
        private PlayerInputActions _characterInputs;

        private Rigidbody2D _rb;
        private Transform _tr;
        //private Animator _animator;
        private SpriteRenderer _spriteRen;

        [SerializeField] private CharacterData characterData;
        [SerializeField] private Projectile usedProjectile;
        [SerializeField] private GameObject mousePos;

        #region Variables
        private Vector2 _direction;
        public Vector2 aim;
        [HideInInspector] public float nextTimeCast;
        [SerializeField] private bool isShootingGamepad;
        [SerializeField] private bool isShootingMouse;
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
        }

        private void Update()
        {
            HandleMovement();
            AttackCooldown();
            
            if(isShootingGamepad || isShootingMouse) usedProjectile.CharacterShooting(this, _tr.position);
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
                if(isShootingGamepad) aim = ctx.ReadValue<Vector2>();
            };
            
            _characterInputs.Character.ShootGamepad.performed += ctx => isShootingGamepad = true;
            _characterInputs.Character.ShootGamepad.canceled += ctx => isShootingGamepad = false;
            _characterInputs.Character.ShootMouse.performed += ctx => isShootingMouse = true;
            _characterInputs.Character.ShootMouse.canceled += ctx => isShootingMouse = false;
            
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

        public bool AttackCooldown()
        {
            if(Time.time > nextTimeCast) return true;
            return false;
        }

        public void MousePosition()
        {
            aim = mousePos.transform.position;
        } 
        
    }
}
