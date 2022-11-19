using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public static EnemyDetection instance;
    private BoxCollider2D _col;
    public List<EnemyHealth> enemiesInSight;

    private float _countdown;

    [SerializeField]
    private float timeToCheck;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;

        _col = GetComponent<BoxCollider2D>();
        
        enemiesInSight.Clear();
    }

    private void OnEnable()
    {
        enemiesInSight.Clear();
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
            enemiesInSight.Add(col.gameObject.GetComponent<EnemyHealth>());
        }
    }
}
