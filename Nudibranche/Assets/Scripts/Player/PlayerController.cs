using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        
        #region Variables

        private Rigidbody2D rb;
        
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
        }

        private void OnDisable()
        {
            _move.Disable();
        }

        void Update()
        {
            _moveDirection = _move.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
        }
    }

}
