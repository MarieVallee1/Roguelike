using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public static EnemyDetection instance;
    public List<GameObject> enemiesInSight;
    private BoxCollider2D _col;

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

        _countdown = 0;
        enemiesInSight.Clear();
    }

    private void OnEnable()
    {
        enemiesInSight.Clear();
        _col.enabled = true;
        _countdown = timeToCheck;
    }
    
    private void OnDisable()
    {
        _col.enabled = false;
    }

    private void Update()
    {
        SelfDeactivation();
    }

    private void SelfDeactivation()
    {
        _countdown -= Time.deltaTime;

        if (_countdown <= 0) enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Moule"))
        {
            enemiesInSight.Add(col.gameObject);
        }
    }
}
