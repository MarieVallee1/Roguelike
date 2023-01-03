using System;
using System.Collections.Generic;
using GenPro;
using UnityEngine;

namespace UI
{
    public class ArrowManager : MonoBehaviour
    {
        public static ArrowManager Instance;
        
        [SerializeField] private ArrowPointer[] enemyArrows;
        [SerializeField] private ArrowPointer[] entryArrows;
        [SerializeField] private ArrowPointer bossArrow;
        [SerializeField] private ArrowPointer[] characterArrows;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            Instance = this;
        }

        public void SetSpecials(Transform bossPos, List<Transform> characterPos)
        {
            // bossArrow.SetTarget(bossPos,false);
            // for (var i = 0; i < characterPos.Count; i++)
            // {
            //     characterArrows[i].SetTarget(characterPos[i],false);
            // }
        }

        public void SetEnemies(IReadOnlyList<ActivateEnemy> enemyList)
        {
            for (var i = 0; i < enemyList.Count; i++)
            {
                enemyArrows[i].SetTarget(enemyList[i].transform,true);
            }
        }

        public void SetEntries(Transform[] entries)
        {
            MaskEntries();
            for (var i = 0; i < entries.Length; i++)
            {
                entryArrows[i].SetTarget(entries[i],false);
            }
        }

        public void MaskEntries()
        {
            foreach (var arrow in entryArrows)
            {
                arrow.MaskArrow();
            }
        }
    }
}
