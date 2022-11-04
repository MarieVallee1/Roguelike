using System;
using System.Collections;
using System.Collections.Generic;
using Projectiles;
using UnityEngine;

public class DodgeBullet : MonoBehaviour
{
    private GameObject bullet;
    private Vector2 direction;
    [SerializeField] private int dodgeFrequence = 3;
    public int bulletDetected;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerProjectile"))
        {
            bullet = col.gameObject;
            direction = col.GetComponent<BasicAttack>().direction;
            
            Debug.DrawRay(bullet.transform.position, direction * 3, Color.black);

            if (Physics2D.Raycast(bullet.transform.position, direction, 3, LayerMask.GetMask("ProjectileHitEnemy")))
            {
                bulletDetected += 1;
                if (bulletDetected == dodgeFrequence)
                {
                    bulletDetected = 0;
                }
            }
        }
    }
}
