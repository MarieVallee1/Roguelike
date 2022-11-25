using Character;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class FireRateUp : Reward
    {
        [SerializeField] private float fireRateBonus;
        
        public override void OnAcquire()
        {
            //A revoir avec l'intégration du sytème
            PlayerController.Instance.fireRate -= fireRateBonus;
        }
    }
}
