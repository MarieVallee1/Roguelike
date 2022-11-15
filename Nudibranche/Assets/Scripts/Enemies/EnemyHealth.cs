using System;
using System.Collections;
using System.Collections.Generic;
using GenPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Animator animator;
    public int maxPV = 3;
    public int pv;
    public bool vulnerable = true;

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
            animator.SetTrigger("TakeDamage");

            if (pv <= 0)
            {
                GetComponent<ActivateEnemy>().Die();
                gameObject.SetActive(false);
            }
        }
    }
}
