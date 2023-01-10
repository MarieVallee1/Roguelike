namespace Objects.PassiveObject
{
    public class HealthUp : Reward
    {
        public override void OnAcquire()
        {
            Health.instance.GainSpecialHeart();
        }
    }
}
