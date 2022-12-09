using Character;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class FastReload : Reward
    {
        [SerializeField] private float fireRateBonus;
        
        public override void OnAcquire()
        {
            PlayerController.Instance.fireRate -= fireRateBonus;
        }
    }
}
