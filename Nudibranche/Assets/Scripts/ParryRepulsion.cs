using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryRepulsion : MonoBehaviour
{
    public static ParryRepulsion instance;
    private CircleCollider2D _col;
    public List<Rigidbody2D> enemiesNear;

    private float _countdown;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;

        _col = GetComponent<CircleCollider2D>();
        
        enemiesNear.Clear();
    }

    private void OnEnable()
    {
        enemiesNear.Clear();
        _col.enabled = true;
    }
    
    private void OnDisable()
    {
        _col.enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Moule"))
        {
            enemiesNear.Add(col.gameObject.GetComponent<Rigidbody2D>());
        }
    }
}
