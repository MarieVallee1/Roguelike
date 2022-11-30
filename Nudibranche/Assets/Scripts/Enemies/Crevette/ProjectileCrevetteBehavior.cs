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

    private float _countdown;
    public float projectileLifeTime;

    private void Update()
    {
        ProjectileLifeTime();
    }

    private void OnEnable()
    {
        _countdown = 0f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            target = col.gameObject;
            target.GetComponentInParent<PlayerController>().TakeDamage(crevetteProjectileData.damage);
        }

        if (col.CompareTag("Player") || col.CompareTag("Environment"))
        {
            gameObject.SetActive(false);
        }
    }
    
    void ProjectileLifeTime()
    {
        _countdown += Time.deltaTime;
        
        if (_countdown > crevetteProjectileData.projectileLifeTime)
        {
            gameObject.SetActive(false);
        }
    }
}
