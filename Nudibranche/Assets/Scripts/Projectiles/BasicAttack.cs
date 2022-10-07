using Character;
using UnityEngine;

namespace Projectiles
{
    public class BasicAttack : MonoBehaviour
    {
        private int _damages;
        public int multiplier;
        private float _countdown;
        public float projectileDuration;

        private SpriteRenderer _ren;

        private void OnEnable()
        {
            _ren = this.GetComponent<SpriteRenderer>();
            
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
