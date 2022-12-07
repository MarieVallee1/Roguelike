using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class BossDamages : MonoBehaviour
{
    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerController.Instance.TakeDamage(damage);
        }
    }
}
