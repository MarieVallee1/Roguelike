using System;
using Character;
using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu(fileName = "Data", menuName = "Projectile")]

    public class Projectile : ScriptableObject
    {
        public void CharacterShooting(PlayerController cc, Vector2 initialPos)
        {
            for (int i = 0; i < blastLenght; i++)
            {
                GameObject usedProjectile = PoolingSystem.instance.GetObject(projectileName);

                if (usedProjectile != null && cc.AttackCooldown() && cc.BlastCooldown())
                {
                    //Placement & activation
                    usedProjectile.transform.position = initialPos;
                    direction = cc.aim.normalized;
                    usedProjectile.SetActive(true);
            
                    //Physic
                    usedProjectile.GetComponent<Rigidbody2D>().velocity = cc.aim.normalized * projectileSpeed;
                

                    cc.nextTimeCast = Time.time + fireRate;
                }
                return;
            }
            cc.nextTimeBlast = Time.time + cooldown;
        }

        public Vector2 direction;
    
        [Header("Projectile Type")] 
        public string projectileName;
        public string userName;
        [TextArea] public string description;

        [Header("Characteristics")]
        [Range(0,10)] public int blastLenght;
        [Range(0,10)] public float fireRate;
        public float projectileSpeed;
        [Range(0,10)] public float cooldown;
        [Range(0, 100)] public int damage;
    }
}