using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GenPro
{
    public class RoomManager : MonoBehaviour
    {
        public bool activated, roomIsCleared;

        [SerializeField] private EntranceTrigger[] entries;
        [SerializeField] private GameObject[] levelDesign;
        [SerializeField] private GameObject[] background;
        [SerializeField] private GameObject door;

        private GameObject[] _children;
        private List<GameObject> _enemyList;
        private EnemyManager _enemyManager;
        private float _lastPos;

        private void Start()
        {
            _children = new GameObject[2];
            _children[0] = Instantiate(levelDesign[Random.Range(0, levelDesign.Length)], transform);
            _children[0].GetComponent<EnemyManager>().linkedRoom = this;
            _children[1] = Instantiate(background[Random.Range(0, background.Length)], transform);
            _children[1].GetComponent<EnemyManager>().linkedRoom = this;

            foreach (var entry in entries)
            {
                entry.linkedRoom = this;
            }
        }

        public void Activate()
        {
            foreach (var array in _children)
            {
                array.SetActive(true);
            }
            activated = true;
        }

        public void Deactivate()
        {
            foreach (var array in _children)
            {
                array.SetActive(false);
            }
            activated = false;
        }

        public void AddEnemyToList(GameObject enemy)
        {
            _enemyList.Add(enemy);
        }

        public void RemoveEnemy(GameObject enemy)
        {
            _enemyList.Remove(enemy);
            if (_enemyList.Count != 0) return;
            door.SetActive(false);
            roomIsCleared = true;
        }

        public void SummonDoor()
        {
            door.SetActive(true);
            foreach (var enemy in _enemyManager.chosenSpawn.enemies)
            {
                enemy.GetComponent<ActivateEnemy>().enabled = true;
            }
        }
    }
}
