using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxPV = 3;
    public int pv;

    private void OnEnable()
    {
        pv = maxPV;
    }

    public void takeDamage(int damage)
    {
        if (pv <= 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            pv -= damage;
        }
    }
}
