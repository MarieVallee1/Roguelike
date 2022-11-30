using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ennemy;

namespace CrevetteProjectiles
{
    [CreateAssetMenu(fileName = "Data", menuName = "CrevetteProjectile")]
    public class CrevetteProjectile : ScriptableObject
    {
        public void CrevetteShooting(Crevette crevette,Vector2 initialPos, Vector2 aim)
        {
            GameObject usedCrevetteProjectile = PoolingSystem.instance.GetObject(crevetteProjectileName);

            if (usedCrevetteProjectile != null)
            {
                //Placement & activation
                usedCrevetteProjectile.transform.position = initialPos;
                usedCrevetteProjectile.SetActive(true);
                
                // Physics
                usedCrevetteProjectile.GetComponent<Rigidbody2D>().velocity = aim.normalized * projectileSpeed;
            }
        }
    
        [Header("Projectile Type")] 
        public string crevetteProjectileName;
        public string userName;
        [TextArea] public string description;

        [Header("Characteristics")]
        [Range(0,100)] public float projectileSpeed;
        [Range(0, 100)] public int damage;
        [Range(0, 100)] public int projectileLifeTime;
        
    } 
}