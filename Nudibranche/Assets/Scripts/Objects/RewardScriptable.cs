using UnityEngine;

namespace Objects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Reward", order = 1)]
    public class RewardScriptable : ScriptableObject
    {
        public string objectName;
        public Sprite objectImage;
        public bool consumable;
        public int objectPrice;
        [TextArea(15,20)]
        public string objectDescription;
    }
}