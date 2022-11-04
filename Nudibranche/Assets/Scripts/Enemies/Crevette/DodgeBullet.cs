using System;
using System.Collections;
using System.Collections.Generic;
using Projectiles;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DodgeBullet : MonoBehaviour
{
    [Header("Detect Projectile")]
    private GameObject bullet;
    private Vector2 direction;
    [SerializeField] private int dodgeFrequence = 3;
    [SerializeField] int bulletDetected;

    [Header("Dodge")] 
    private bool canDodgeRight;
    private bool canDodgeLeft;
    [SerializeField] private float obstacleDetection = 2;
    [SerializeField] Transform crevettePos;
    [SerializeField] private Rigidbody2D rb;
    public List<Vector2> dashDirection = new();
    [SerializeField] private float dashSpeed = 50;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerProjectile"))
        {
            bullet = col.gameObject;
            direction = col.GetComponent<BasicAttack>().direction;

            if (Physics2D.Raycast(bullet.transform.position, direction, 3, LayerMask.GetMask("ProjectileHitEnemy")))
            {
                bulletDetected += 1;
                if (bulletDetected == dodgeFrequence)
                {
                    Dodge();
                    bulletDetected = 0;
                }
            }
        }
    }

    void Dodge()
    {
        dashDirection.Clear();
        if (Physics2D.Raycast(crevettePos.position, direction.Perpendicular1(), obstacleDetection, LayerMask.GetMask( "Obstacle", "Canonnier","Oursins", "Player")))
        {
            Debug.DrawRay(crevettePos.position, direction.Perpendicular1()*obstacleDetection, Color.black);
            canDodgeLeft = false;
        }
        else
        {
            canDodgeLeft = true;
            dashDirection.Add(direction.Perpendicular1());
        }
        
        if (Physics2D.Raycast(crevettePos.position, direction.Perpendicular2(), obstacleDetection, LayerMask.GetMask( "Obstacle", "Canonnier","Oursins", "Player")))
        {
            Debug.DrawRay(crevettePos.position, direction.Perpendicular2()*obstacleDetection, Color.black);
            canDodgeRight = false;
        }
        else
        {
            canDodgeRight = true;
            dashDirection.Add(direction.Perpendicular2());
        }

        if (dashDirection.Count != 0)
        {
            rb.AddForce(dashDirection[Random.Range(0,dashDirection.Count)] * dashSpeed, ForceMode2D.Impulse);
        }
    }
}
