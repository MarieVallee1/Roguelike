using System.Collections;
using System.Collections.Generic;
using Character;
using Narration;
using UnityEngine;

public class BackgroundInteraction : MonoBehaviour
{
    
    private bool _inZone;


    private void Update()
    {
        if (_inZone)
        {
            InteractionZone();
        }
    }
    
    void InteractionZone()
    {
        if (PlayerController.instance.characterInputs.Character.Interact.triggered)
        {
            PlayerController.instance.FreezeCharacter();
        }
            
        if (PlayerController.instance.characterInputs.UI.Interact.triggered)
        {
            
        }
    }
}
