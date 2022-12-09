using System;
using System.Collections.Generic;
using Character;
using Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GenPro
{
    public class RoomManager : MonoBehaviour
    {
        public bool activated, roomIsCleared;

        [SerializeField] private bool isBig, character;
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
            var index = Random.Range(0, levelDesign.Length);
            if (character)
            {
                if (index == GameManager.instance.firstCharacterIndex)
                {
                    if (index == 0) index++;
                    else index--;
                }
                GameManager.instance.firstCharacterIndex = index;
            }
            _levelDesign = Instantiate(levelDesign[index], transform);
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
            GameManager.instance.inCombat = false;
            door.SetActive(false);
            roomIsCleared = true;
        }

        public void SummonDoor()
        {
            GameManager.instance.AddScore();
            if (_enemyList.Count ==0) roomIsCleared = true;
            else
            {
                GameManager.instance.inCombat = true;
                ItemManager.Instance.OnRoomEntrance();
                door.SetActive(true);
                foreach (var enemy in _enemyList)
                {
                    enemy.Activate();
                }
                var aStar = AstarPath.active;
                if (isBig) aStar.data.gridGraph.SetDimensions(42,42,1);
                else aStar.data.gridGraph.SetDimensions(20,20,1);
                aStar.data.gridGraph.center = transform.position;
                aStar.data.graphs[0].Scan();
            }
        }
    }
}
