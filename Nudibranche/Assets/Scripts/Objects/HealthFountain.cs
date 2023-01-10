using Character;

namespace Objects
{
    public class HealthFountain : Reward
    {
        public override void OnAcquire()
        {
            var player = PlayerController.Instance;
            var maxHealth = Health.instance.numberOfHearts;
            if (player.health < maxHealth) player.health++;
            Health.instance.SetHealth(player.health);
        }
    }
}
