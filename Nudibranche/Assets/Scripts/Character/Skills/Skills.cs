using System.Collections.Generic;
using UnityEngine;

namespace Character.Skills
{
    [CreateAssetMenu(menuName = "Skill")]
    public class Skills : ScriptableObject
    {
        [Header("0 = Scie Rano")]
        [Header("1 = Shellock")]
        [Header("2 = Sirène")]
        [Space]
        public int skillIndex;
        [Header("Ability Cooldown")]
        public float cooldown;

        public void UseCurrentSkill()
        {
            switch (skillIndex)
            {
                case 0 : SwordSlash();
                    break;
                case 1 : WrongTrack();
                    break;
                case 2 : CardLaser();
                    break;
            }
        }

        private void SwordSlash()
        {
            //Compétence ScieRano
            EnemyDetection.instance.enabled = true;
            for (int i = 0; i < EnemyDetection.instance.enemiesInSight.Count; i++)
            {
                EnemyDetection.instance.enemiesInSight[i].GetComponent<EnemyHealth>().takeDamage(3);
            }
            Debug.Log(1);
        }
        private void WrongTrack()
        {
            //Compétence Shellock
        }
        private void CardLaser()
        {
            //Compétence Sirène
        }
    }
}
