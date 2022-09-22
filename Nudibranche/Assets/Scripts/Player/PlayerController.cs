using Objects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        
        #region Variables

        private Rigidbody2D _rb;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletPosition;
        
        public PlayerInputActions playerControls;
        
        private InputAction _move;
        private InputAction _fire;
        private InputAction _look;
        
        private Vector2 _moveDirection = Vector2.zero;
        public Vector2 aimDirection;
        

        public float moveSpeed;

        #endregion

        private void Awake()
        {
            playerControls = new PlayerInputActions();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _move = playerControls.Player.Move;
            _move.Enable();

            _fire = playerControls.Player.Fire;
            _fire.Enable();
            
            _look = playerControls.Player.Look;
            _look.Enable();
        }

        private void OnDisable()
        {
            _move.Disable();
            _fire.Disable();
            _look.Disable();
        }

        void Update()
        {
            _moveDirection = _move.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            _rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
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
                aimDirection = _look.ReadValue<Vector2>();
                
                bullet.transform.position = bulletPosition.position;
                bullet.SetActive(true);
            }
        }
    }

}
