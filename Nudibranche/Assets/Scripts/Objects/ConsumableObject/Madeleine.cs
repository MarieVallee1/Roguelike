using System;
using Character;
using UnityEngine;

namespace Objects.ConsumableObject
{
    public class Madeleine : Consumable
    {
        public override void OnUse()
        {
            var player = PlayerController.Instance;
            var maxHealth = player.characterData.health;
            if (player.health < maxHealth) player.health = maxHealth;
        }
    }
}
