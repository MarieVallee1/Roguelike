using UnityEngine;

namespace Character
{
    [CreateAssetMenu(menuName = "Data", fileName = "PlayerData", order = 0)]
    public class CharacterData : ScriptableObject
    {
        [Range(0f, 100f)]
        public float speed;
        [Range(0f, 100f)]
        public float maxSpeed;
        [Range(0f, 10f)]
        public float drag;
        [Range(0f, 100f)]
        public int health;
        [Range(0f, 100f)]
        public float parryCooldown;        
        [Range(0f, 100f)]
        public float parryTime;
        [Range(0f, 100f)]
        public float buffDuration;
        [Range(0f, 50f)]
        public float repulsionForce;
    }
}
