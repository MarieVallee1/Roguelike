using System.Collections;
using System.Collections.Generic;
using Character;
using Narration;
using UnityEngine;

public class BossCinematicTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    private bool _inZone;

    private void Update()
    {
        if (_inZone && PlayerController.Instance.characterInputs.UI.Interact.triggered)
        { 
            DialogueManager.instance.ContinueDialogue(dialogue);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _inZone = true;
        InteractionZone();
    }  
    private void OnTriggerExit2D(Collider2D other)
    {
        _inZone = false;
    }

    void InteractionZone()
    {
        DialogueManager.instance.StartDialogue(dialogue);
        PlayerController.Instance.DisableInputs();
    }
}
