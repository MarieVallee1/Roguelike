using System;
using Player;
using TMPro;
using UnityEngine;

    public class Bullet : MonoBehaviour
    {

        private float _speed = 30f;
        [SerializeField] private PlayerController pC;
        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            pC = FindObjectOfType<PlayerController>();
            rb.velocity = pC.aimDirection * _speed;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                gameObject.SetActive(false);
            }        
        }
    }
