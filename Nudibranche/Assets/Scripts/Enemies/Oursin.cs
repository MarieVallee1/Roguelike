using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ennemy;
using Objects;

namespace Oursins
{
    [CreateAssetMenu(fileName = "Data", menuName = "Oursin")]

    public class Oursin : ScriptableObject
    {
        public void CannonierShooting(Vector2 spawnPos)
        {
            GameObject usedOursin = PoolingSystem.instance.GetObject(oursinName);

            if (usedOursin != null)
            {
                Debug.Log(usedOursin);
                //Placement & activation
                usedOursin.transform.position = spawnPos;
                usedOursin.SetActive(true);
            }
        }
    
        [Header("Projectile Type")] 
        public string oursinName;
        public string userName;
        [TextArea] public string description;

        [Header("Characteristics")]
        [Range(0, 100)] public int explosionDamage;
        [Range(0, 100)] public int passiveDamage;

        public float radius;
    }
}
