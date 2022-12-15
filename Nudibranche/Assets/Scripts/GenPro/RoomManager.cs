using System;
using System.Collections.Generic;
using Character;
using Objects;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GenPro
{
    public class RoomManager : MonoBehaviour
    {
        public bool activated, roomIsCleared;

        [SerializeField] private bool isBig, character;
        [SerializeField] private GameObject[] levelDesign;
        [SerializeField] private GameObject[] deactivate;
        [SerializeField] private GameObject door;
        public GameObject pearlStack;

        public List<ActivateEnemy> _enemyList = new();
        public Transform[] entries;
        
        private GameObject _levelDesign;
        private GameObject _background;
        private float _lastPos;
        private bool _arrowSet;

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
        }

        public void Activate()
        {
            _levelDesign.SetActive(true);
            foreach (var item in deactivate)
            {
                item.SetActive(true);
            }
            activated = true;
        }

        public void Deactivate()
        {
            _levelDesign.SetActive(false);
            foreach (var item in deactivate)
            {
                item.SetActive(false);
            }
            activated = false;
        }

        public void AddEnemyToList(ActivateEnemy enemy)
        {
            _enemyList.Add(enemy);
        }

        public void RemoveEnemy(ActivateEnemy enemy)
        {
            _enemyList.Remove(enemy);

            if (_enemyList.Count < 5)
            {
                if (!_arrowSet)
                {
                    ArrowManager.Instance.SetEnemies(_enemyList);
                    _arrowSet = true;
                }
            }
            
            if (_enemyList.Count != 0) return;
            GameManager.instance.inCombat = false;
            door.SetActive(false);
            roomIsCleared = true;
            ArrowManager.Instance.SetEntries(entries);
        }

        public void SummonDoor()
        {
            if (roomIsCleared) return;
            
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

        public void SetArrows()
        {
            ArrowManager.Instance.SetEntries(entries);
        }
    }
}
