using System;
using UnityEngine;

namespace Projectiles
{
    public class BasicAttack : MonoBehaviour
    {
        private int _damages;
        private float _countdown;
        public float projectileDuration;

        private void OnEnable()
        {
            _countdown = 0f;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
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
