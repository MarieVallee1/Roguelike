using Character;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class StrengthUp : Reward
    {
        [SerializeField] private float damageMultiplier;
        public override void OnAcquire()
        {
            PlayerController.instance.damage *= damageMultiplier;
        }
    }
}
