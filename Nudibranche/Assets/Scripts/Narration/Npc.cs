using System;
using Character;
using UnityEngine;
using DG.Tweening;

namespace Narration
{
    public class Npc : MonoBehaviour
    {
        [SerializeField] private Dialogue dialogue;
        [SerializeField] private SpriteRenderer interactLogo;
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
            interactLogo.DOFade(1, 0.3f);
        }  
        private void OnTriggerExit2D(Collider2D other)
        {
            _inZone = false;
            interactLogo.DOFade(0, 0.3f);
        }

        void InteractionZone()
        {
            if (PlayerController.Instance.characterInputs.Character.Interact.triggered)
            {
                DialogueManager.instance.StartDialogue(dialogue);
                PlayerController.Instance.DisableInputs();
                interactLogo.DOFade(0, 0.3f);
            }
            
            if (PlayerController.Instance.characterInputs.UI.Interact.triggered)
            {
                DialogueManager.instance.ContinueDialogue(dialogue);
            }
        }
    }
}
