using System;
using Character;
using UnityEngine;
using Projectiles;

namespace Projectiles
{
    public class BasicAttack : MonoBehaviour
    {
        private int _damages;
        private float _countdown;
        
        public int multiplier;
        public float projectileDuration;
        public Vector2 direction;

        private SpriteRenderer _ren;
        private TrailRenderer _trail;

        public Projectile projectileData;
        
        private void OnEnable()
        {
            _ren = GetComponent<SpriteRenderer>();

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
            if (other.gameObject.CompareTag("Enemy") && other.gameObject.CompareTag("Environment"))
            {
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            ProjectileLifeTime();
        }

        void ProjectileLifeTime()
        {
            _countdown += Time.deltaTime;
            if (_countdown > projectileDuration)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
