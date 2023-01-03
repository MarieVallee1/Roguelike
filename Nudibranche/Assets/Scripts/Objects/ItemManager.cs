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
        
        [Header("Références objets")]
        [SerializeField] private List<Reward> inShop;
        public Reward health;
        [Header("Tableaux d'activation")]
        [SerializeField] private Reward[] onRoomEntrance;
        [SerializeField] private Reward[] onEnemyHit;
        [SerializeField] private Reward[] onObstacleHit;
        [SerializeField] private Reward[] onPlayerDeath;
        [SerializeField] private Reward[] onEnemyDeath;

        //Singleton
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            Instance = this;
        }

        //Item calls
        
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

        //Various methods
        public Reward RandomPassiveItem()
        {
            var reward = inShop[Random.Range(0, inShop.Count)];
            if (inShop.Count>1) inShop.Remove(reward);
            return reward;
        }
    }
}
