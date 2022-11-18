using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager Instance;
        
        [SerializeField] private List<Reward> inShop;
        [SerializeField] private Reward[] consumable;
        [SerializeField] private Reward[] onRoomEntrance;
        [SerializeField] private Reward[] onPlayerDeath;
        [SerializeField] private Reward[] onEnemyDeath;
        [SerializeField] private Reward[] onUse;

        //Singleton
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            Instance = this;
        }

        //Item calls
        public void OnUse(int index)
        {
            onUse[index].OnUse();
        }

        public void OnRoomEntrance()
        {
            foreach (var item in onRoomEntrance)
            {
                if(item.isOwned) item.OnRoomEntrance();
            }
        }

        public void OnPlayerDeath()
        {
            foreach (var item in onPlayerDeath)
            {
                if(item.isOwned) item.OnPlayerDeath();
            }
        }
        
        public void OnEnemyDeath()
        {
            foreach (var item in onEnemyDeath)
            {
                if(item.isOwned) item.OnEnemyDeath();
            }
        }
        
        //Shop calls
        private int _slot;
        
        public Reward PickItem()
        {
            _slot++;
            return _slot switch
            {
                1 => RandomConsumable(),
                2 => Random.Range(0, 2) == 0 ? RandomConsumable() : RandomPassiveItem(),
                _ => RandomPassiveItem()
            };
        }

        //Various methods
        public Reward RandomConsumable()
        {
            return consumable[Random.Range(0, consumable.Length)];
        }

        public Reward RandomPassiveItem()
        {
            var reward = inShop[Random.Range(0, inShop.Count)];
            if (inShop.Count>1) inShop.Remove(reward);
            return reward;
        }
    }
}
