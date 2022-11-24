using Character;

namespace Objects
{
    public class HealthFountain : Reward
    {
        public override void OnAcquire()
        {
            PlayerController.Instance.health += 1;
        }
    }
}
