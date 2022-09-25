using Objects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        #region Variables
      
        private Vector2 _movement;
        private Vector2 _aim;
        
        private float _nextFireTime;
        
        public bool isAttacking;
        
        #endregion
        
        #region Declaration
        
        [SerializeField] private PlayerData playerData;
        [SerializeField] private Transform bulletPosition;
        private Rigidbody2D _rb;
        private PlayerInputActions _playerControls;

        #endregion

        private void Awake()
        {
            _playerControls = new PlayerInputActions();
            _rb = GetComponent<Rigidbody2D>();
        }
        private void OnEnable()
        {
            _playerControls.Enable();
            
            _playerControls.Player.Movement.performed += OnMovement;
            _playerControls.Player.Aim.performed += OnAim;
            _playerControls.Player.Shoot.performed += ctx => isAttacking = true;
            _playerControls.Player.Shoot.canceled += ctx => isAttacking = false;
        }
        private void OnDisable()
        {
            _playerControls.Disable();
        }
        private void Update()
        {
            BasicAttackCooldown();

            if (isAttacking)
            {
                BasicAttack();
            }
        }
        private void FixedUpdate()
        {
            HandleMovement();
            HandleRotation();
        }
        void BasicAttack()
        {
            GameObject bullet = ObjectPooling.instance.GetPooledObject();

            if (bullet != null && BasicAttackCooldown())
            {
                bullet.transform.position = bulletPosition.position;
                bullet.SetActive(true);
                bullet.GetComponent<Rigidbody2D>().velocity = _aim.normalized * playerData.basicAttackSpeed;
                _nextFireTime = Time.time + playerData.basicAttackCooldown;
            }
        }
        void OnMovement(InputAction.CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>();
        }
        void OnAim(InputAction.CallbackContext context)
        {
            _aim = (context.ReadValue<Vector2>());
        }
        void HandleMovement()
        {
            _rb.velocity = new Vector2(_movement.x * playerData.characterSpeed, _movement.y * playerData.characterSpeed);
        }
        void HandleRotation()
        {
            float a = Mathf.Atan2(_aim.x, _aim.y) * Mathf.Rad2Deg;
            _rb.MoveRotation(-a);
        }
        bool BasicAttackCooldown()
        {
            if(Time.time > _nextFireTime) return true;
            return false;
        }

    }

}
