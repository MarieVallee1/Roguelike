using Character;

namespace Objects
{
    public class HealthFountain : Reward
    {
        public override void OnAcquire()
        {
            PlayerController.instance.health += 1;
        }
    }
}
