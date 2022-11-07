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
        [SerializeField] private GameObject blackScreen;

        private GameObject[] _children;
        private List<ActivateEnemy> _enemyList;
        private float _lastPos;

        private void Start()
        {
            _children = new GameObject[2];
            //_children[0] = Instantiate(levelDesign[Random.Range(0, levelDesign.Length)], transform);
            // _children[0].GetComponent<EnemySpawn>().ChooseSpawn(this);
            // _children[1] = Instantiate(background[Random.Range(0, background.Length)], transform);

            foreach (var entry in entries)
            {
                entry.linkedRoom = this;
            }
        }

        public void Activate()
        {
            // foreach (var array in _children)
            // {
            //     array.SetActive(true);
            // }
            blackScreen.SetActive(false);
            activated = true;
        }

        public void Deactivate()
        {
            // foreach (var array in _children)
            // {
            //     array.SetActive(false);
            // }
            blackScreen.SetActive(true);
            activated = false;
        }

        public void AddEnemyToList(ActivateEnemy enemy)
        {
            _enemyList.Add(enemy);
        }

        public void RemoveEnemy(ActivateEnemy enemy)
        {
            _enemyList.Remove(enemy);
            if (_enemyList.Count != 0) return;
            door.SetActive(false);
            roomIsCleared = true;
        }

        public void SummonDoor()
        {
            // door.SetActive(true);
            // foreach (var enemy in _enemyList)
            // {
            //     enemy.Activate();
            // }
        }
    }
}
