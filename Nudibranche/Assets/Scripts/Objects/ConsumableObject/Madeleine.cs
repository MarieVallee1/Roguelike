using Character;

namespace Objects.ConsumableObject
{
    public class Madeleine : Consumable
    {
        public override void OnUse()
        {
            var player = PlayerController.Instance;
            var maxHealth = player.characterData.health;
            if (player.health < maxHealth) player.health = maxHealth;
            Health.instance.SetHealth(player.health);
        }
    }
}
