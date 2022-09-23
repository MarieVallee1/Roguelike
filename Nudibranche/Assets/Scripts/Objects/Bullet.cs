using System;
using Player;
using TMPro;
using UnityEngine;

namespace Objects
{ 
    public class Bullet : MonoBehaviour
    {
        private Rigidbody2D _rb;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                gameObject.SetActive(false);
            }        
        }
    }
}

