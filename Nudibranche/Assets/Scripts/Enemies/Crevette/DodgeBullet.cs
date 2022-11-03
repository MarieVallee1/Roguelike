using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBullet : MonoBehaviour
{
    private GameObject bullet;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerProjectile"))
        {
            Debug.Log("dodge");
        }
    }
}
