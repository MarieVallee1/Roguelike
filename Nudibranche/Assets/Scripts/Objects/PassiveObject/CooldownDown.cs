using Character;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class CooldownDown : Reward
    {
        [SerializeField] private float cooldownDivider;
        
        public override void OnAcquire()
        {
            PlayerController.Instance.skills.cooldownReduction = cooldownDivider;
        }
    }
}
