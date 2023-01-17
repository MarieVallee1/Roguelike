using System;
using System.Collections;
using Character;
using Narration;
using UnityEngine;

public class BossCinematicTrigger : MonoBehaviour
{
    public static BossCinematicTrigger instance;
    
    [SerializeField] private Dialogue dialogue;
    
    private Camera _cam;
    public Boss bossScript;
    
    private bool _inZone;

    
    private void Awake()
    {
        if(instance != null && instance != this) Destroy(this);
        instance = this;
    }
    private void Start()
    {
        _cam = Camera.current;
    }
    private void Update()
    {
        if (PlayerController.Instance.characterInputs.UI.Interact.triggered && _inZone)
        {
            DialogueManager.instance.ContinueDialogue(dialogue);
        }
    }


    public void CoroutineLauncher()
    {
        StartCoroutine(BeginBossCinematic());
    }
    
    private IEnumerator BeginBossCinematic()
    {
        //Prevent the boss from attacking directly the player
        
        bossScript.enabled = false;
        
        PlayerController.Instance.DisableInputs();
        yield return new WaitForSeconds(.5f);
        PlayerController.Instance.FreezeCharacter();
        
        //Allow the lookahead to know when to perform the camera translation
        GameManager.instance.inBossCutscene = true;
        
        yield return new WaitForSeconds(1.5f);
        
        DialogueManager.instance.StartDialogue(dialogue);
        _inZone = true;
    }
    public IEnumerator StartBossFight()
    {
        GameManager.instance.inBossCutscene = false;
        
        yield return new WaitForSeconds(1f);
        
        bossScript.enabled = true;
    }
    
    
}
