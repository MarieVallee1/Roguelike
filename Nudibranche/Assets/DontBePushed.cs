using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontBePushed : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
