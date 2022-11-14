using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GenPro
{
    public class RoomManager : MonoBehaviour
    {
        public bool activated, roomIsCleared;

        [SerializeField] private GameObject[] levelDesign;
        [SerializeField] private GameObject[] background;
        [SerializeField] private GameObject door;
        [SerializeField] private GameObject blackScreen;

        private GameObject _levelDesign;
        private GameObject _background;
        private List<ActivateEnemy> _enemyList = new();
        private float _lastPos;

        private void Start()
        {
            _levelDesign = Instantiate(levelDesign[Random.Range(0, levelDesign.Length)], transform);
            _levelDesign.GetComponent<EnemySpawn>().ChooseSpawn(this);
            // _background = Instantiate(background[Random.Range(0, background.Length)], transform);
        }

        public void Activate()
        {
            _levelDesign.SetActive(true);
            //_background.SetActive(true);
            blackScreen.SetActive(false);
            activated = true;
        }

        public void Deactivate()
        {
            _levelDesign.SetActive(false);
            //_background.SetActive(false);
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
            door.SetActive(true);
            foreach (var enemy in _enemyList)
            {
                enemy.Activate();
            }
        }
    }
}
