using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using Random = UnityEngine.Random;

public class PerleBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Vector2 randomDirection;
    [SerializeField] private float speed = 2;
    [SerializeField] private Perle pearlData;
    private void OnEnable()
    {
        randomDirection = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)).normalized;
        rb.AddForce(randomDirection*speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (pearlData.monney == true)
            {
                GameManager.instance.pearlAmount += 1;
                GameManager.instance.pearlAmountText.text = GameManager.instance.pearlAmount + "";
            }
            else
            {
                if (Health.instance.health < Health.instance.numberOfHearts)
                {
                    Health.instance.SetHealth(PlayerController.instance.health += 1);
                    Debug.Log("Le joueur récupère un PV");
                }
            }
            
            gameObject.SetActive(false);
        }
    }
}
