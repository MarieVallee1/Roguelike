using Character;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class HitAndRun : Reward
    {
        [SerializeField] private float damageMultiplier;
        [SerializeField] private int maxStreak;

        private float _baseDamage;
        private int _currentStreak;
        
        public override void OnEnemyHit()
        {
            if (_currentStreak == maxStreak) return;
            _currentStreak++;
            PlayerController.instance.damage = _baseDamage * damageMultiplier * _currentStreak;
        }

        public override void OnObstacleHit()
        {
            _currentStreak = 0;
            PlayerController.instance.damage = _baseDamage;
        }

        public override void ResetStat()
        {}
    }
}
