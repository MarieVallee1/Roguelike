using System;
using Objects;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        private Camera _cam;
        
        private Rigidbody2D _rb;
        [SerializeField] private Transform bulletPosition;

        private PlayerInputActions _playerControls;
        private InputAction _shoot;
        private Vector2 _movement;
        private Vector2 _aim;

        public float rightStickDeadzone; 
        public float moveSpeed;
        public float projectileSpeed;
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
            _playerControls.Player.Movement.canceled += OnMovement;
            _playerControls.Player.Aim.performed += OnAim;
            _playerControls.Player.Shoot.performed += ctx => OnShoot();
            _playerControls.Player.Shoot.canceled += ctx => OnShoot();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleRotation();
        }

        private void Update()
        {
            Debug.Log(_aim);
        }

        void OnShoot()
        {
            GameObject bullet = ObjectPooling.instance.GetPooledObject();
            
            if (bullet != null)
            {
                bullet.transform.position = bulletPosition.position;
                bullet.SetActive(true);
                bullet.GetComponent<Rigidbody2D>().velocity = _aim.normalized * projectileSpeed;
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
            _rb.velocity = new Vector2(_movement.x * moveSpeed, _movement.y * moveSpeed);
        }

        void HandleRotation()
        {
            float a = Mathf.Atan2(_aim.x, _aim.y) * Mathf.Rad2Deg;
            _rb.MoveRotation(-a);
        }
    }

}
