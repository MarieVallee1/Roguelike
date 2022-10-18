using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GenPro
{
    public class EnemyManager : MonoBehaviour
    {
        public RoomManager linkedRoom;
        [SerializeField] private EnemySpawn[] enemySpawn;
        public EnemySpawn chosenSpawn;

        private void Awake()
        {
            chosenSpawn = enemySpawn[Random.Range(0, enemySpawn.Length)];
            foreach (var enemy in chosenSpawn.enemies)
            {
                linkedRoom.AddEnemyToList(enemy);
            }
        }
    }
}
