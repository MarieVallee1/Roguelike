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
    private PlayerController _playerController;

    private void Start()
    {
        target = PlayerController.Instance.gameObject;
        _playerController = PlayerController.Instance;
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
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && dangerous)
        {
            target = other.gameObject;

            if (_playerController.onParry)
            {
                _playerController.TakeDamage(oursinData.damage);
                Disappear();
            }
            else _playerController.TakeDamage(oursinData.damage);
        }
    }
}
