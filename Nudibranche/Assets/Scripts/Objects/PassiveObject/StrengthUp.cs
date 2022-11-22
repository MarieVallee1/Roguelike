using Character;
using Projectiles;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class StrengthUp : Reward
    {
        [SerializeField] private Projectile buffedProjectile;
        public override void OnAcquire()
        {
            PlayerController.instance.usedProjectile = buffedProjectile;
        }
    }
}
