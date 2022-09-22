using System;
using Objects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        
        #region Variables

        private Rigidbody2D rb;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletPosition;
        
        public PlayerInputActions playerControls;
        
        private InputAction _move;
        private InputAction _fire;
        
        private Vector2 _moveDirection = Vector2.zero;

        public float moveSpeed;

        #endregion

        private void Awake()
        {
            playerControls = new PlayerInputActions();
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _move = playerControls.Player.Move;
            _move.Enable();

            _fire = playerControls.Player.Fire;
            _fire.Enable();
        }

        private void OnDisable()
        {
            _move.Disable();
            _fire.Disable();
        }

        void Update()
        {
            _moveDirection = _move.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
        }

        void OnFire()
        {
            Fire();
        }

        void Fire()
        {
            GameObject bullet = ObjectPooling.instance.GetPooledObject();

            if (bullet != null)
            {
                bullet.transform.position = bulletPosition.position;
                bullet.SetActive(true);
            }
        }
    }

}
