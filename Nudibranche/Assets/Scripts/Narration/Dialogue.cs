using UnityEngine;

namespace Narration
{
    
    [CreateAssetMenu (menuName = "Dialogue")]
    public class Dialogue : ScriptableObject
    {
        public string name;
        
        [TextArea(3, 10)]
        public string[] sentences;
    }
}
