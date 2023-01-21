using System;
using Ennemy;
using UnityEngine;

namespace GenPro
{
    public class ActivateEnemy : MonoBehaviour
    {
        private EnemySpawn _levelDesign;
        private bool _isSet;
        public enum Enemy
        {
            moule,
            crevette,
            canonnier,
            boss
        }

        public Enemy enemy;
        [SerializeField] private IAMoule scriptMoule;
        [SerializeField] private Crevette scriptCrevette;
        [SerializeField] private Cannonier scriptCanonnier;
        [SerializeField] private Boss scriptBoss;

        public void SetReference(EnemySpawn callingScript)
        {
            _levelDesign = callingScript;
            _isSet = true;
        }
        
        public void Activate()
        {
            switch (enemy)
            {
                case Enemy.moule:
                    scriptMoule.enabled = true;
                    break;
                case Enemy.crevette:
                    scriptCrevette.enabled = true;
                    break;
                case Enemy.canonnier:
                    scriptCanonnier.enabled = true;
                    break;
                case Enemy.boss:
                    BossCinematicTrigger.instance.bossScript = scriptBoss;
                    BossCinematicTrigger.instance.CoroutineLauncher();
                    GameManager.instance.bossCinematicPos = transform.position;
                    break;
            }
        }

        public void Die()
        {
            //Increase the kill counter in the GameManager
            switch (enemy)
            {
                case Enemy.moule:
                    GameManager.instance.mouleKilled += 1;
                    break;
                case Enemy.crevette:
                    GameManager.instance.crevetteKilled += 1;
                    break;
                case Enemy.canonnier:
                    GameManager.instance.cannonierKilled += 1;
                    break;
                case Enemy.boss:
                    GameManager.instance.bossKilled = true;
                    break;
            }
            
            if(_isSet)_levelDesign.linkedRoom.RemoveEnemy(this);
        }

        public void BaitDetection(Transform baitTransform)
        {
            switch (enemy)
            {
                case Enemy.moule:
                    scriptMoule.StartCoroutineTarget(baitTransform);
                    break;
                case Enemy.crevette:
                    scriptCrevette.StartCoroutineTarget(baitTransform);
                    break;
                case Enemy.canonnier:
                    scriptCanonnier.StartCoroutineTarget(baitTransform);
                    break;
                case Enemy.boss:
                    scriptBoss.StartCoroutineTarget(baitTransform);
                    break;
            }
        }
    }
}
