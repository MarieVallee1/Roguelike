using System;
using Character;
using Ennemy;
using GenPro;
using UnityEngine;

public class ParryRepulsion : MonoBehaviour
{
    public static ParryRepulsion instance;
    private CircleCollider2D _col;
    private ActivateEnemy _enemyType;
    

    private float _countdown;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;

        _col = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        _col.enabled = true;
    }
    private void OnDisable()
    {
        _col.enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 13) return;
        if (col.gameObject.layer == 14) return;
        Debug.Log("Pass");
        if (!PlayerController.instance.isParrying) return;
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (col.gameObject.layer is 11 or 17) return;
            Test(col);
        }

        if (col.gameObject.CompareTag("Moule"))
        {
            if (col.gameObject.layer != 9) return;
            Test(col);
        }
    }

    private void Test(Collider2D col)
    {
        _enemyType = col.GetComponent<ActivateEnemy>();
        switch (_enemyType.enemy)
        {
            case ActivateEnemy.Enemy.crevette:
                col.GetComponent<Crevette>().stopPathfinding = false;
                break;
            case ActivateEnemy.Enemy.moule:
                col.GetComponent<IAMoule>().stopPathfinding = false;
                break;
        }
        Vector2 dir = (Vector2)col.transform.position - PlayerController.instance.characterPos;
        col.GetComponent<Rigidbody2D>().AddForce(dir.normalized * PlayerController.instance.characterData.repulsionForce,ForceMode2D.Impulse);
    }
}
