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

        [SerializeField] private bool isBig, character, characterMusic;
        [SerializeField] private GameObject[] levelDesign;
        [SerializeField] private GameObject[] deactivate;
        [SerializeField] private GameObject backgroundSouth;
        [SerializeField] private GameObject door;
        public GameObject pearlStack;
        public SpriteRenderer minimapIcon;
        [SerializeField] private Sprite iconSprite;

        public List<ActivateEnemy> _enemyList = new();
        public EntranceTrigger[] entries;
        
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
                    if (index == 0) index = levelDesign.Length - 1;
                    else index--;
                }
                GameManager.instance.firstCharacterIndex = index;
            }
            minimapIcon.sprite = iconSprite;
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

        public void DeactivateBackground()
        {
            if(backgroundSouth!=default) backgroundSouth.SetActive(false);
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
            SetArrows();
            AudioList.Instance.StartMusic(AudioList.Music.main,true);
            AudioList.Instance.inCombat = false;
        }

        public void SummonDoor()
        {
            Minimap.Minimap.Instance.MinimapUpdate(transform);
            
            //Afficher fond et sprite de mur transparent ICI
            //Tous les objets supplémentaires doivent être ajouté à Deactivate[] dans le prefab de Room
            
            if(backgroundSouth!=default) backgroundSouth.SetActive(true);
            
            if (roomIsCleared) return;
            
            GameManager.instance.AddScore();
            minimapIcon.gameObject.SetActive(true);

            if (_enemyList.Count ==0) roomIsCleared = true;
            else
            {
                AudioList.Instance.StartMusic(_enemyList[0].enemy==ActivateEnemy.Enemy.boss?AudioList.Music.boss : AudioList.Music.combat,true);
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

        public void SetTriggers()
        {
            foreach (var entry in entries)
            {
                entry.SetTrigger();
            }
        }
        
        public void ResetTriggers()
        {
            foreach (var entry in entries)
            {
                entry.ResetTrigger();
            }
        }

        public void IsCharacter(bool inside)
        {
            if (!characterMusic) return;
            AudioList.Instance.StartMusic(inside ? AudioList.Music.character : AudioList.Music.main, true);
        }
        
        //For TP
        public void ResetRoom()
        {
            ResetTriggers();
            foreach (var enemy in _enemyList)
            {
                enemy.Deactivate();
            }
            door.SetActive(false);
        }
    }
}
