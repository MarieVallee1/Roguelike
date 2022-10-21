using UnityEngine;

namespace Narration
{
    
    [CreateAssetMenu (menuName = "Dialogue")]
    public class Dialogue : ScriptableObject
    {
        public new string name;
        
        [TextArea(3, 10)]
        public string[] sentences;
        [TextArea(3, 10)]
        public string[] choice1Branch;
        [TextArea(3, 10)]
        public string[] choice2Branch;
        
        public string[] choices;
    }
}
