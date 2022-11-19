using Character;
using Enemies;
using UnityEngine;

namespace Projectiles
{
    public class BasicAttack : MonoBehaviour
    {
        private int _damages;
        private float _countdown;
        
        public int multiplier;
        public float projectileDuration;
        [SerializeField] private AnimationCurve acceleration;
        public Vector2 direction;

        private SpriteRenderer _ren;
        private TrailRenderer _trail;
        private Rigidbody2D _rb;

        public Projectile projectileData;

        private void Awake()
        {
            _ren = GetComponent<SpriteRenderer>();
            _trail = GetComponent<TrailRenderer>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _trail.enabled = true;
            _countdown = 0f;

            if (PlayerController.instance.isBuffed)
            {
                _damages *= multiplier;
                _ren.color = Color.red;
            }
            else
            {
                _ren.color = Color.white;
            }
            
            direction = projectileData.direction;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponentInParent<EnemyHealth>().takeDamage(projectileData.damage);
            }
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Environment"))
            {
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            ProjectileLifeTime();
            Acceleration();
        }

        void ProjectileLifeTime()
        {
            _countdown += Time.deltaTime;
            if (_countdown > projectileDuration/3)
            {
                _trail.enabled = false;
            }
            if (_countdown > projectileDuration)
            {
                gameObject.SetActive(false);
            }
        }
        
        void Acceleration()
        {
            _rb.AddForce(PlayerController.instance.aim.normalized * acceleration.Evaluate(1));
        }
    }
}
