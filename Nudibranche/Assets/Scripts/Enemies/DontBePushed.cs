using System;
using UnityEngine;

namespace Enemies
{
    public class DontBePushed : MonoBehaviour
    {
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
}
