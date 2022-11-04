using System;
using System.Collections;
using System.Collections.Generic;
using Projectiles;
using UnityEngine;

public class DodgeBullet : MonoBehaviour
{
    private GameObject bullet;
    private Vector2 direction;

    private bool bulletDetected;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerProjectile"))
        {
            bulletDetected = true;
            bullet = col.gameObject;
            direction = col.GetComponent<BasicAttack>().direction;

            if (Physics2D.Raycast(col.transform.position, direction, 5, LayerMask.GetMask("ProjectileHitEnnemy")))
            {
                Debug.Log("Dodge)");
            }
        }
    }
}
