using Character;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class ProjectileSizeUp : Reward
    {
        [SerializeField] private float projectileSizeMultiplier;
        
        public override void OnAcquire()
        {
            PlayerController.Instance.projectileSize *= projectileSizeMultiplier;
        }
    }
}
