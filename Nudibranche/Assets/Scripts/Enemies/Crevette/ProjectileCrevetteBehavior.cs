using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCrevetteBehavior : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        gameObject.SetActive(false);
    }
}
