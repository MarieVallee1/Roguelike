using System.Collections;
using Character;
using UnityEngine;

namespace Objects.ConsumableObject
{
    public class FioleCourage : Consumable
    {
        [SerializeField] private float buffDuration;
        [SerializeField] private int damageBonus;
        [SerializeField] private float fireRateBonus;
        
        public override void OnUse()
        {
            isOwned = false;
            StartCoroutine(Buff());
        }

        private IEnumerator Buff()
        {
            var player = PlayerController.Instance;
            player.damage += damageBonus;
            player.fireRate -= fireRateBonus;
            player.onBuff = true;
            yield return new WaitForSeconds(buffDuration);
            player.damage -= damageBonus;
            player.fireRate += fireRateBonus;
            player.onBuff = false;
        }
    }
}
