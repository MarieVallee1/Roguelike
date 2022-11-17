namespace Objects.PassiveObject
{
    public class TestPassiveObject : Reward
    {
        public RewardScriptable stats;
        
        public override int GetPrice()
        {
            return 1;
        }
    }
}
