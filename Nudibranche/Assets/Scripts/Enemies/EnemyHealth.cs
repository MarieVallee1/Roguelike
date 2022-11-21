using System;
using GenPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        public Animator[] animators;
        public ParticleSystem fxDamages;
    
        [SerializeField] private Enemy enemy;
        [SerializeField] private Perle perleBlancheData;
        [SerializeField] private Perle perleRougeData;
    
        private GameManager _gameManager;
        private enum Enemy
        {
            moule,
            crevette,
            canonnier
        }
    
        public int maxPV = 3;
        public int pv;
        public bool vulnerable = true;

        private void Start()
        {
            _gameManager = GameManager.instance;
        }

        private void OnEnable()
        {
            pv = maxPV;
            vulnerable = true;
        }
    
        public void takeDamage(int damage)
        {
            if (vulnerable)
            {
                pv -= damage;
                fxDamages.Play();

                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].SetTrigger("TakeDamage");
                }

                if (pv <= 0)
                {
                    DropLoot();
                    GetComponent<ActivateEnemy>().Die();
                    gameObject.SetActive(false);
                }
            }
        }
        private void DropLoot()
        {
            switch (enemy)
            {
                case Enemy.moule:
                    for (int i = 0; i < _gameManager.moulePearlDrop; i++)
                    {
                        perleBlancheData.LootDrop((Vector2)transform.position);
                    }

                    if (Random.Range(0f, 1f) <= _gameManager.mouleLifeDrop)
                    {
                        perleRougeData.LootDrop(transform.position);
                    }
                    break;
                case Enemy.crevette:
                    for (int i = 0; i < _gameManager.crevettePearlDrop; i++)
                    {
                        perleBlancheData.LootDrop((Vector2)transform.position);
                    }
                    if (Random.Range(0f, 1f) <= _gameManager.crevetteLifeDrop)
                    {
                        perleRougeData.LootDrop(transform.position);
                    }
                    break;
                case Enemy.canonnier:
                    for (int i = 0; i < _gameManager.canonnierPearlDrop; i++)
                    {
                        perleBlancheData.LootDrop((Vector2)transform.position);
                    }
                    if (Random.Range(0f, 1f) <= _gameManager.canonnierLifeDrop)
                    {
                        perleRougeData.LootDrop(transform.position);
                    }
                    break;
            }
        }
    }
}
