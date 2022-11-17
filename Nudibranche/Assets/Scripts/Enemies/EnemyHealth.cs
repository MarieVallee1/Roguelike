using System;
using System.Collections;
using System.Collections.Generic;
using GenPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour
{
    public Animator[] animators;
    public int maxPV = 3;
    public int pv;
    public bool vulnerable = true;
    public ParticleSystem fxDamages;
    [SerializeField] private GameManager gameManager;

    private enum Enemy
    {
        moule,
        crevette,
        canonnier
    }

    [SerializeField] private Enemy enemy;
    [SerializeField] private Perle perleBlancheData;
    [SerializeField] private Perle perleRougeData;

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
                for (int i = 0; i < gameManager.moulePearlDrop; i++)
                {
                    perleBlancheData.LootDrop((Vector2)transform.position);
                }
                break;
            case Enemy.crevette:
                for (int i = 0; i < gameManager.crevettePearlDrop; i++)
                {
                    perleBlancheData.LootDrop((Vector2)transform.position);
                }
                break;
            case Enemy.canonnier:
                for (int i = 0; i < gameManager.canonnierLifeDrop; i++)
                {
                    perleBlancheData.LootDrop((Vector2)transform.position);
                }
                break;
        }
    }
}
