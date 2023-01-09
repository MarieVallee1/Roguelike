using UnityEngine;

namespace Narration
{
    
    [CreateAssetMenu (menuName = "Dialogue")]
    public class Dialogue : ScriptableObject
    {
        public new string name;
        [Space]
        public bool noChoiceDialogue;
        [TextArea(3, 10)]
        public string[] sentences;
        [TextArea(3, 10)]
        public string[] choice1Branch;
        [TextArea(3, 10)]
        public string[] choice2Branch;
        public string[] choices;

        [Header("PnJ Skill")]
        [Tooltip("0 : Sirène de coeur || 1 : Scie Rano || 2 : Shellock || 3+ : Null ")]
        public int skillIndex;
        
    }
}
