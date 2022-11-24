using System;
using System.Collections;
using System.Collections.Generic;
using Ennemy;
using Projectiles;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DodgeBullet : MonoBehaviour
{
    [Header("Detect Projectile")]
    private GameObject bullet;
    private Vector2 direction;
    private Vector2 dir;

    [Header("Dodge")] 
    private bool canDodgeRight;
    private bool canDodgeLeft;
    [SerializeField] private float obstacleDetection = 2;
    [SerializeField] Transform crevettePos;
    [SerializeField] private Rigidbody2D rb;
    private List<Vector2> dashDirection = new();
    [SerializeField] private float dashSpeed = 50;
    private Vector2 dodgeRight;
    private Vector2 dodgeLeft;
    private Vector2 dodgeDirection;
    public Crevette crevette;
    public float dodgeFrequence = 1;
    private float timer;

    [Header("Visuels")] [SerializeField] 
    private Animator[] animators;

    private void OnEnable()
    {
        timer = dodgeFrequence;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerProjectile"))
        {
            bullet = col.gameObject;
            direction = col.GetComponent<BasicAttack>().direction;

            if (Physics2D.Raycast(bullet.transform.position, direction, 3, LayerMask.GetMask("ProjectileHitEnemy")))
            {
                if (timer >= dodgeFrequence)
                {
                    Dodge();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
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
            dodgeLeft = direction.Perpendicular1();
            dashDirection.Add(dodgeLeft);
        }
        
        if (Physics2D.Raycast(crevettePos.position, direction.Perpendicular2(), obstacleDetection, LayerMask.GetMask( "Obstacle", "Canonnier","Oursins", "Player")))
        {
            Debug.DrawRay(crevettePos.position, direction.Perpendicular2()*obstacleDetection, Color.black);
            canDodgeRight = false;
        }
        else
        {
            canDodgeRight = true;
            dodgeRight = direction.Perpendicular2();
            dashDirection.Add(dodgeRight);
        }

        if (dashDirection.Count != 0)
        {
            crevette.spriteRotation = false;
            dodgeDirection = dashDirection[Random.Range(0, dashDirection.Count)];
            rb.AddForce(dodgeDirection * dashSpeed, ForceMode2D.Impulse);

            if (dodgeDirection == dodgeRight)
            {
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].SetBool("DodgeRight", true);
                }
            }
            else if (dodgeDirection == dodgeLeft)
            {
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].SetBool("DodgeLeft", true);
                }
            }
        }

        timer = 0;
    }
}
