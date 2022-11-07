using Ennemy;
using UnityEngine;

namespace GenPro
{
    public class ActivateEnemy : MonoBehaviour
    {
        [SerializeField] private EnemySpawn levelDesign;
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
            levelDesign.linkedRoom.RemoveEnemy(this);
        }
    }
}
