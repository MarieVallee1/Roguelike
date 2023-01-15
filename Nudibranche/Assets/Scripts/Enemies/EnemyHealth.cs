using System;
using System.Collections;
using GenPro;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Ennemy;

namespace Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        public Animator[] animators;
        public ParticleSystem fxDamages;
        public Rigidbody2D rb;
        public Collider2D collider;
        [SerializeField] private Enemy enemy;
        [SerializeField] private Perle perleBlancheData;
        [SerializeField] private Perle perleRougeData;
        private GameManager _gameManager;
        public SpriteRenderer[] sprites;
        private float shaderDissolveValue = 1;
        public float dissolveDuration = 1;
        private bool dead;
        [SerializeField] private Material[] charaMat;
        
        //Audio
        [SerializeField] private AudioSource audioSource;

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
            dead = false;
            shaderDissolveValue = 1;
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].material.SetFloat("_Dissolve", shaderDissolveValue);
            }
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].enabled = true;
            }
        }
    
        public void takeDamage(int damage)
        {
            if (vulnerable && !dead)
            {
                StartCoroutine(HitFeedback());
                pv -= damage;
                fxDamages.Play();

                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].SetTrigger("TakeDamage");
                }

                if (pv <= 0)
                {
                    dead = true;
                    StartCoroutine(Death());
                }
            }
        }

        private IEnumerator HitFeedback()
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].material = charaMat[1];
            }

            yield return new WaitForSeconds(0.1f);
            
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].material = charaMat[0];
            }
            
            yield return new WaitForSeconds(0.1f);
        }

        private void Update()
        {
            if (dead)
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].material.SetFloat("_Dissolve", shaderDissolveValue);
                }
            }
        }

        private IEnumerator Death()
        {
            audioSource.PlayOneShot(AudioList.Instance.enemyDeath);
            
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].material = charaMat[0];
            }
            
            collider.enabled = false;
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].enabled = false;
            }
            DropLoot();
            DOTween.To(()=> shaderDissolveValue, x=> shaderDissolveValue = x, -1, dissolveDuration);
            yield return new WaitForSeconds(dissolveDuration);
            GetComponent<ActivateEnemy>().Die();
            gameObject.SetActive(false);
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
