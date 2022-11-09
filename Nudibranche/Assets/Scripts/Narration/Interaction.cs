using System;
using System.Collections.Generic;
using UnityEngine;

namespace Narration
{
    [CreateAssetMenu (menuName = "Interactions")]
    public class Interaction : ScriptableObject
    {
        [TextArea] public List<String> sentences;
    }
}
