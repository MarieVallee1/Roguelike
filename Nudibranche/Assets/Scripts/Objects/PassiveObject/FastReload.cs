using Character;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class FastReload : Reward
    {
        [SerializeField] private float reloadBonus;
        
        public override void OnAcquire()
        {
            PlayerController.Instance.reloadCountdown -= reloadBonus;
        }
    }
}
