using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class IAMoule : MonoBehaviour
{
    private float oldXPos;
    private float newXPos;
    private SpriteRenderer mouleSprite;

    private void Start()
    {
        mouleSprite = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        oldXPos = transform.position.x;
    }

    void Update()
    {
        Flip();
    }

    void Flip()
    {
        newXPos = transform.position.x;
        if (oldXPos<=newXPos)
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
