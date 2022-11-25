using Character;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class ProjectileSizeUp : Reward
    {
        [SerializeField] private float projectileSizeBonus;
        
        public override void OnAcquire()
        {
            PlayerController.Instance.projectileSize += projectileSizeBonus;
        }
    }
}
