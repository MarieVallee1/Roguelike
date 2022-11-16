using System;
using System.Collections;
using System.Collections.Generic;
using GenPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Animator[] animators;
    public int maxPV = 3;
    public int pv;
    public bool vulnerable = true;
    public ParticleSystem fxDamages;

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
                GetComponent<ActivateEnemy>().Die();
                gameObject.SetActive(false);
            }
        }
    }
}
