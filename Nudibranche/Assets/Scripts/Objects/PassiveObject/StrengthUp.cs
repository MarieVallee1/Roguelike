using Character;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class StrengthUp : Reward
    {
        [SerializeField] private int damageBonus;
        public override void OnAcquire()
        {
            PlayerController.Instance.damage += damageBonus;
        }
    }
}
