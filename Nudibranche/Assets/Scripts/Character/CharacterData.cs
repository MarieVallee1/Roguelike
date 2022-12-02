using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(menuName = "Data", fileName = "PlayerData", order = 0)]
    public class CharacterData : ScriptableObject
    {
        [Header("Player")]
        [Range(0f, 100f)]
        public float speed;
        [Range(0f, 100f)]
        public float maxSpeed;
        [Range(0f, 10f)]
        public float drag;
        [Range(0f, 10f)]
        public int health;
        [Range(0f, 10f)]
        public int invulnerabilityDuration;

        [Header("Projectile")]
        [Tooltip("Define the element you're using in the list")]
        public int projectileIndex;
        public List<ProjectileType> usedProjectile;
        [Space]
        
        [Header("Parry")]
        [Range(0f, 10)]
        public float parryCooldown; 
        [Range(0f, 10)]
        public float parryTime;
        [Range(0f, 10f)]
        public float buffDuration;
        public float speedBuff;
        [Range(0f, 50f)]
        public float repulsionForce;

        [Header("Skills")] 
        public float swordSlashCooldown;
        public float cardLaserCooldown;
        public float baitCooldown;
        
        public int swordSlashDamages;
        public int cardLaserDamages;
        public float baitDuration;
        
    }

    [Serializable]
    public class ProjectileType
    {
        [Header("Projectiles")]
        public string usedProjectileName;
        [Range(0f, 10f)]
        public float fireRate;
        [Range(0f, 10f)]
        public float projectileSize;
        [Range(0f, 50f)]
        public float projectileSpeed;
        [Range(0f, 10f)]
        public int damage;
        [Range(0f, 10f)]
        public int blastLenght;
        [Range(0f, 10f)]
        public float blastCooldown;
        public AnimationCurve projectileAcceleration;
        public float duration;
        public int damageMultiplier;
    }
}
