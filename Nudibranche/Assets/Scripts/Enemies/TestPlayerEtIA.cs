using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerEtIA : MonoBehaviour
{
    public int pv = 5;

    public void TakeDamage(int damage)
    {
        pv -= damage;
    }
}
