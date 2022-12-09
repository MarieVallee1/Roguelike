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
        [SerializeField] private int consumableLuckDivider;

        [Header("Consommable actuel")]
        public int lastConsumable = -1;
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

        //Singleton
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            Instance = this;

            var i = 0;
            for (; i < consumable.Length; i++)
            {
                consumable[i].consumableIndex = i;
            }
        }

        //Item calls
        public void OnUse()
        {
            if (lastConsumable == -1) return;
            consumable[lastConsumable].OnUse();
            lastConsumable = -1;
            UIManager.instance.UpdateObjectInfo();
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
                2 => Random.Range(0, consumableLuckDivider) == 0 ? RandomConsumable() : RandomPassiveItem(),
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
            if (lastConsumable!=-1) SpawnConsumable(lastConsumable);
            lastConsumable = newIndex;
        }

        public void SpawnConsumable(Vector3 position)
        {
            Instantiate(RandomConsumable().gameObject, position, Quaternion.identity,GameManager.instance.transform);
        }
        
        public void SpawnConsumable(int index)
        {
            Instantiate(consumable[index].gameObject, PlayerController.Instance.transform.position, Quaternion.identity,GameManager.instance.transform);
        }

        public Reward ConsumableInfo()
        {
            return consumable[lastConsumable];
        }
    }
}
