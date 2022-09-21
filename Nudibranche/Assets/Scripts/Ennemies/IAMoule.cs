using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMoule : MonoBehaviour
{
    public GameObject player;
    public float speed = 1;
    private SpriteRenderer mouleSprite;
    private float oldXPos;
    private float newXPos;

    private void Start()
    {
        mouleSprite = gameObject.GetComponent<SpriteRenderer>();
        oldXPos = transform.position.x;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
        Flip();

    }

    void Flip()
    {
        newXPos = transform.position.x;
        if (oldXPos<newXPos)
        {
            mouleSprite.flipX = true;
        }
        else
        {
            mouleSprite.flipX = false;
        }

        oldXPos = newXPos;
    }
}
