using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager Instance;

        [Header("Consommable actuel")]
        [SerializeField] private int lastConsumable = -1;
        [Header("Références objets")]
        [SerializeField] private List<Reward> inShop;
        [SerializeField] private Reward[] consumable;
        public Reward health;
        [Header("Tableaux d'activation")]
        [SerializeField] private Reward[] onRoomEntrance;
        [SerializeField] private Reward[] onEnemyHit;
        [SerializeField] private Reward[] onObstacleHit;
        [SerializeField] private Reward[] onPlayerDeath;
        [SerializeField] private Reward[] onEnemyDeath;
        [SerializeField] private Reward[] onUse;

        //Singleton
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            Instance = this;

            var i = 0;
            for (; i < inShop.Count; i++)
            {
                inShop[i].consumableIndex = i;
            }
        }

        //Item calls
        public void OnUse(int index)
        {
            onUse[index].OnUse();
            lastConsumable = -1;
        }

        public void OnRoomEntrance()
        {
            foreach (var item in onRoomEntrance)
            {
                if(item.isOwned) item.OnRoomEntrance();
            }
        }

        public void OnEnemyHit()
        {
            foreach (var item in onEnemyHit)
            {
                if(item.isOwned) item.OnEnemyHit();
            }
        }

        public void OnObstacleHit()
        {
            foreach (var item in onObstacleHit)
            {
                if(item.isOwned) item.OnObstacleHit();
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

        public void CheckConsumable(int newIndex)
        {
            if (lastConsumable!=newIndex) SpawnConsumable(lastConsumable);
            lastConsumable = newIndex;
        }
        
        public void SpawnConsumable(int index)
        {
            Instantiate(onUse[index].gameObject, PlayerController.Instance.transform.position, Quaternion.identity);
        }
    }
}
