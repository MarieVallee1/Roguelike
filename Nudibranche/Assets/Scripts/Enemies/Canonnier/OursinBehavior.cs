using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Oursins;
using UnityEngine;
using DG.Tweening;

public class OursinBehavior : MonoBehaviour
{
    public Oursin oursinData;
    private bool dangerous = false;
    private GameObject target;

    private void Start()
    {
        target = PlayerController.instance.gameObject;
    }

    private void OnEnable()
    {
        dangerous = false;
    }

    public void IsDangerous()
    {
        dangerous = true;
    }

    public void Disappear()
    {
        dangerous = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && dangerous)
        {
            target = other.gameObject;
            target.GetComponent<PlayerController>().TakeDamage(oursinData.damage);
        }
    }
}
