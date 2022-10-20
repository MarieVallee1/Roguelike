using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using CrevetteProjectiles;

public class ProjectileCrevetteBehavior : MonoBehaviour
{
    public CrevetteProjectile crevetteProjectileData;
    private GameObject target;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            target = col.gameObject;
            target.GetComponentInParent<PlayerController>().TakeDamage(crevetteProjectileData.damage);
        }

        gameObject.SetActive(false);
    }
}
