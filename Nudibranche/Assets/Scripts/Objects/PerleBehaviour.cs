using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class PerleBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Vector2 randomDirection;
    [SerializeField] private float speed = 2;
    [SerializeField] private Perle pearlData;
    private Transform target;
    public float speedTowardPlayer = 10;
    private bool getPearl;
    [SerializeField] private Collider2D collider;

    private void Start()
    {
        target = PlayerController.Instance.transform;
    }

    private void OnEnable()
    {
        randomDirection = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)).normalized;
        rb.AddForce(randomDirection*speed, ForceMode2D.Impulse);
        getPearl = false;
        collider.enabled = true;
    }

    private void Update()
    {
        if (getPearl)
        {
            MoveTowardPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.CompareTag("Player"))
        {
            if ((!pearlData.monney && PlayerController.Instance.health < Health.instance.numberOfHearts) || pearlData.monney)
            {
                getPearl = true;
                UIManager.instance.PearlUpFeedback();
            }
        }
    }

    private void MoveTowardPlayer()
    {
        collider.enabled = false;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speedTowardPlayer * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.position) <= 0.2)
        {
            getPearl = false;
            if (pearlData.monney)
            {
                GameManager.instance.pearlAmount += 1;
                GameManager.instance.pearlAmountText.text = GameManager.instance.pearlAmount + "";
                AudioList.Instance.PlayOneShot(AudioList.Instance.pearlCollect,AudioList.Instance.pearlCollectVolume);
            }
            else
            {
                if (PlayerController.Instance.health < Health.instance.numberOfHearts)
                {
                    Health.instance.SetHealth(PlayerController.Instance.health += 1);
                    AudioList.Instance.PlayOneShot(AudioList.Instance.playerHeal,AudioList.Instance.playerHealVolume);
                }
            }
            gameObject.SetActive(false);
        }
    }
}
