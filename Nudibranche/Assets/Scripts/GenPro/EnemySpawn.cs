using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GenPro
{
    public class EnemySpawn : MonoBehaviour
    {
        public RoomManager linkedRoom;
        [SerializeField] private Spawn[] spawns;
        public Spawn chosenSpawn;

        public void ChooseSpawn()
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
        public ActivateEnemy[] enemies;
    }
}
