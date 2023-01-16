using Character;

namespace Objects
{
    public class HealthFountain : Reward
    {
        public override void OnAcquire()
        {
            Health.instance.SetHealth(PlayerController.Instance.health += 1);
        }
    }
}
