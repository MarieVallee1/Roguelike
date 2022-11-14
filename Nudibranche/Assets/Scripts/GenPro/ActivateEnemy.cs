using Ennemy;
using UnityEngine;

namespace GenPro
{
    public class ActivateEnemy : MonoBehaviour
    {
        private EnemySpawn _levelDesign;
        private enum Enemy
        {
            moule,
            crevette,
            canonnier,
            boss
        }

        [SerializeField] private Enemy enemy;
        [SerializeField] private IAMoule scriptMoule;
        [SerializeField] private Crevette scriptCrevette;
        [SerializeField] private Cannonier scriptCanonnier;
        //[SerializeField] private Boss scriptBoss;

        public void SetReference(EnemySpawn callingScript)
        {
            _levelDesign = callingScript;
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
                    //scriptBoss.enabled = true;
                    break;
            }
        }

        public void Die()
        {
            _levelDesign.linkedRoom.RemoveEnemy(this);
        }
    }
}
