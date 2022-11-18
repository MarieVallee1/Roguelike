using UnityEngine;

namespace Objects
{
    public class Reward : MonoBehaviour
    {
        public bool isOwned;

        public virtual int GetPrice()
        {
            return 0;
        }
        
        public virtual void OnAcquire()
        {}
        
        public virtual void OnRoomEntrance()
        {}

        public virtual void OnEnemyDeath()
        {}
        
        public virtual void OnPlayerDeath()
        {}

        public virtual void OnUse()
        {}
    }
}
