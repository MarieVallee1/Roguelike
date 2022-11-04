using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
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
            
            if (pv <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
