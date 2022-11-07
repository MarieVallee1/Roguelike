using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GenPro
{
    public class EnemyManager : MonoBehaviour
    {
        public RoomManager linkedRoom;
        [SerializeField] private Spawn[] spawns;
        public Spawn chosenSpawn;

        private void Awake()
        {
            chosenSpawn = spawns[Random.Range(0, spawns.Length)];
            foreach (var enemy in chosenSpawn.enemies)
            {
                linkedRoom.AddEnemyToList(enemy);
            }
        }
    }

    [Serializable]
    public class Spawn
    {
        public GameObject[] enemies;
    }
}
