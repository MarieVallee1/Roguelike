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
        {
            UIManager.instance.UpdateObjectInfo();
        }
        
        public virtual void OnRoomEntrance()
        {}
        
        public virtual void OnEnemyHit()
        {}
        
        public virtual void OnObstacleHit()
        {}

        public virtual void OnEnemyDeath()
        {}

        public virtual void OnPlayerDeath()
        {}

        public virtual void OnUse()
        {}
    }
}
