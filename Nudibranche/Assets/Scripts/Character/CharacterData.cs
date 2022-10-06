using UnityEngine;

namespace Character
{
    [CreateAssetMenu(menuName = "Data", fileName = "PlayerData", order = 0)]
    public class CharacterData : ScriptableObject
    {

        [Range(0f, 100f)]
        public float speed;
        [Range(0f, 100f)]
        public float health;
        [Range(0f, 100f)]
        public float parryCooldown;        
        [Range(0f, 100f)]
        public float parryTime;
        
    
    }
}
