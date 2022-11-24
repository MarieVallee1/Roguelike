using System.Collections;
using System.Collections.Generic;
using Character;
using Narration;
using UnityEngine;

public class BackgroundInteraction : MonoBehaviour
{
    
    private bool _inZone;
    [SerializeField] private Interaction interaction;


    private void OnTriggerEnter2D(Collider2D other)
    {
        _inZone = true;
    }  
    private void OnTriggerExit2D(Collider2D other)
    {
        _inZone = false;
    }
    
    private void Update()
    {
        if (_inZone)
        {
            InteractionZone();
        }
    }
    
    void InteractionZone()
    {
        if (PlayerController.Instance.characterInputs.Character.Interact.triggered)
        {
            InteractionManager.instance.StartDialogue(interaction);
            PlayerController.Instance.DisableInputs();
        }
            
        if (PlayerController.Instance.characterInputs.UI.Interact.triggered)
        {
            InteractionManager.instance.DisplayNextSentence();
        }
    }
}
