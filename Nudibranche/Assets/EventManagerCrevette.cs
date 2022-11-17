using System.Collections;
using System.Collections.Generic;
using Ennemy;
using UnityEngine;

public class EventManagerCrevette : MonoBehaviour
{
    [SerializeField] private Crevette crevette;

    public void Shoot()
    {
        crevette.Shoot();
    }

}
