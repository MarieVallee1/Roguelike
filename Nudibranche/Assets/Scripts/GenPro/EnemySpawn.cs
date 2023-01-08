using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GenPro
{
    public class EnemySpawn : MonoBehaviour
    {
        public RoomManager linkedRoom;
        [SerializeField] private Spawn[] spawns;
        private Spawn _chosenSpawn;
        [SerializeField] private bool character;
        [SerializeField] private GameObject characterIcon;

        public void ChooseSpawn(RoomManager currentRoom)
        {
            linkedRoom = currentRoom;
            if (character) Instantiate(characterIcon, linkedRoom.minimapIcon.transform);
            else
            {
                _chosenSpawn = spawns[Random.Range(0, spawns.Length)];
                foreach (var enemy in _chosenSpawn.enemies)
                {
                    enemy.gameObject.SetActive(true);
                    enemy.SetReference(this);
                    linkedRoom.AddEnemyToList(enemy);
                }
            }
        }
    }

    [Serializable]
    public class Spawn
    {
        public ActivateEnemy[] enemies;
    }
}
