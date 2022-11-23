using Character;
using Enemies;
using Objects;
using UnityEngine;

namespace Projectiles
{
    public class BasicAttack : MonoBehaviour
    {
        private SpriteRenderer _ren;
        private TrailRenderer _trail;
        private Rigidbody2D _rb;
        private CharacterData _characterData;
        
        private int _damages;
        private float _countdown;
    
        public Vector2 direction;

        private void Awake()
        {
            _ren = GetComponent<SpriteRenderer>();
            _trail = GetComponent<TrailRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _characterData = PlayerController.instance.characterData;
        }

        private void OnEnable()
        {
            _trail.enabled = true;
            _countdown = 0f;

            if (PlayerController.instance.isBuffed)
            {
                _damages *= _characterData.usedProjectile[_characterData.projectileIndex].damageMultiplier;
                _ren.color = Color.red;
            }
            else
            {
                _ren.color = Color.white;
            }
            
            direction = PlayerController.instance.shootDir;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponentInParent<EnemyHealth>().takeDamage(_characterData.usedProjectile[_characterData.projectileIndex].damage);
                gameObject.SetActive(false);
                ItemManager.Instance.OnEnemyHit();
            }
            if (other.gameObject.CompareTag("Environment"))
            {
                gameObject.SetActive(false);
                ItemManager.Instance.OnObstacleHit();
            }
            if (other.gameObject.CompareTag("Boss"))
            {
                other.gameObject.GetComponentInParent<Boss>().TakeDamage(_characterData.usedProjectile[_characterData.projectileIndex].damage);
                gameObject.SetActive(false);
                ItemManager.Instance.OnEnemyHit();
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
            if (_countdown > _characterData.usedProjectile[_characterData.projectileIndex].duration/3)
            {
                _trail.enabled = false;
            }
            if (_countdown > _characterData.usedProjectile[_characterData.projectileIndex].duration)
            {
                gameObject.SetActive(false);
            }
        }
        
        void Acceleration()
        {
            _rb.AddForce(PlayerController.instance.aim.normalized * _characterData.usedProjectile[_characterData.projectileIndex].projectileAcceleration.Evaluate(1));
        }
    }
}
