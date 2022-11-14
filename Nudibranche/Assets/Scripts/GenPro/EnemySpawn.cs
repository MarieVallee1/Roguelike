using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GenPro
{
    public class EnemySpawn : MonoBehaviour
    {
        public RoomManager linkedRoom;
        [SerializeField] private Spawn[] spawns;
        [SerializeField] private Spawn _chosenSpawn;

        public void ChooseSpawn(RoomManager currentRoom)
        {
            linkedRoom = currentRoom;
            _chosenSpawn = spawns[Random.Range(0, spawns.Length)];
            foreach (var enemy in _chosenSpawn.enemies)
            {
                Debug.Log(enemy.gameObject.name);
                enemy.gameObject.SetActive(true);
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
