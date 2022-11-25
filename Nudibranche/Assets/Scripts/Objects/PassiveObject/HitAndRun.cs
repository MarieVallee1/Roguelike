using Character;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class HitAndRun : Reward
    {
        [SerializeField] private int damageBonus;
        [SerializeField] private int maxStreak;
        private int _currentStreak;
        
        public override void OnEnemyHit()
        {
            if (_currentStreak == maxStreak) return;
            _currentStreak++;
            PlayerController.Instance.damage += damageBonus;
        }

        public override void OnObstacleHit()
        {
            PlayerController.Instance.damage -= damageBonus*_currentStreak;
            _currentStreak = 0;
        }
    }
}
