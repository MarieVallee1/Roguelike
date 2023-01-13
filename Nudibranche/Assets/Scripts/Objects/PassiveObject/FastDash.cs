using Character;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class FastDash : Reward
    {
        [SerializeField] private int dashCooldownDivider;
        public override void OnAcquire()
        {
            PlayerController.Instance.dashCooldown /= dashCooldownDivider;
        }
    }
}
