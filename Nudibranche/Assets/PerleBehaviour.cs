using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PerleBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 randomDirection;
    [SerializeField] private float speed = 2;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        randomDirection = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)).normalized;
        rb.AddForce(randomDirection*speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameManager.instance.pearlAmount += 1;
            GameManager.instance.pearlAmountText.text = GameManager.instance.pearlAmount + "";
            gameObject.SetActive(false);
        }
    }
}
