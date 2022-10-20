using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Oursins;
using UnityEngine;
using DG.Tweening;

public class OursinBehavior : MonoBehaviour
{
    public float timeBeforeExplosion = 1;
    public float timeOnScreen = 2;
    public Oursin oursinData;
    private Animator animator;
    private bool dangerous = false;
    private GameObject target;
    private bool targetInZone;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(Behavior());
        dangerous = false;
    }

    IEnumerator Behavior()
    {
        yield return new WaitForSeconds(timeBeforeExplosion);
        if (targetInZone)
        {
            target.GetComponent<PlayerController>().TakeDamage(oursinData.explosionDamage);
        }
        animator.SetTrigger("Explode");
        dangerous = true;
        yield return new WaitForSeconds(timeOnScreen);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            target = col.gameObject;
            targetInZone = true;
            
            if (dangerous)
            {
                target.GetComponent<PlayerController>().TakeDamage(oursinData.explosionDamage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target.GetComponent<PlayerController>().TakeDamage(oursinData.passiveDamage);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetInZone = false;
        }
    }
}
