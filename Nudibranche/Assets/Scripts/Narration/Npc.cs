using Character;
using UnityEngine;

namespace Narration
{
    public class Npc : MonoBehaviour
    {
        [SerializeField] private Dialogue dialogue;
        private bool _inZone;

        private void Update()
        {
            if (_inZone)
            {
                InteractionZone();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _inZone = true;
        }  
        private void OnTriggerExit2D(Collider2D other)
        {
            _inZone = false;
        }

        void InteractionZone()
        {
            if (PlayerController.instance.characterInputs.Character.Interact.triggered)
            {
                DialogueManager.instance.StartDialogue(dialogue);
                PlayerController.instance.FreezeCharacter();
            }
            
            if (PlayerController.instance.characterInputs.UI.Interact.triggered)
            {
                DialogueManager.instance.ContinueDialogue(dialogue);
            }
        }
    }
}
