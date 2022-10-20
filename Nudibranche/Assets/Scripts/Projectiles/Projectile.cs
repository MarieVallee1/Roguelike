using Character;
using Objects;
using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu(fileName = "Data", menuName = "Projectile")]

    public class Projectile : ScriptableObject
    {
        public void CharacterShooting(PlayerController cc, Vector2 initialPos)
        {
            GameObject usedProjectile = PoolingSystem.instance.GetObject(projectileName);

            if (usedProjectile != null && cc.AttackCooldown())
            {
                //Placement & activation
                usedProjectile.transform.position = initialPos;
                usedProjectile.SetActive(true);
            
                //Physic
                usedProjectile.GetComponent<Rigidbody2D>().velocity = cc.aim.normalized * projectileSpeed;

                cc.nextTimeCast = Time.time + fireRate;
            }
        }
    
        [Header("Projectile Type")] 
        public string projectileName;
        public string userName;
        [TextArea] public string description;

        [Header("Characteristics")]
        [Range(0,10)] public float fireRate;
        [Range(0,100)] public float projectileSpeed;
        [Range(0,10)] public float cooldown;
        [Range(0, 100)] public int damage;
    }
}