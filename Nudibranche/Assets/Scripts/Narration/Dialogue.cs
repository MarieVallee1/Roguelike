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

        [Header("Npc Index")]
        [Tooltip("0 : Sirène de coeur || 1 : Scie Rano || 2 : Shellock || 3 : Boss Princess || 4 : Grollum ||5+ : Null ")]
        public int npcIndex;
        
    }
}
