using System;
using UnityEngine;

namespace Objects
{
    public class Reward : MonoBehaviour
    {
        public RewardScriptable stats;
        [HideInInspector]public int consumableIndex;
        public bool isOwned;

        public int GetPrice()
        {
            return stats.objectPrice;
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
