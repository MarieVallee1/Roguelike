using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using DG.Tweening;
using GenPro;
using Narration;
using TMPro;
using UnityEngine;

public class BackgroundInteraction : MonoBehaviour
{
    
    private bool _inZone;
    [SerializeField] private Interaction interaction;
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI interactionTxt;
    [SerializeField] private SpriteRenderer eToInteract;
    private Collider2D collider;

    private void Start()
    {
        collider = gameObject.GetComponent<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _inZone = true;
            eToInteract.DOFade(1, 0.3f);
        }
    }  
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("trigger");
            _inZone = false;
            eToInteract.DOFade(0, 0.3f);
        }
    }
    
    private void Update()
    {
        if (_inZone)
        {
            InteractionZone();
        }

        if (GameManager.instance.inCombat)
        {
            collider.enabled = false;
        }
        
        if (!GameManager.instance.inCombat)
        {
            collider.enabled = true;
        }
    }
    
    void InteractionZone()
    {
        if (PlayerController.Instance.characterInputs.Character.Interact.triggered)
        {
            eToInteract.DOFade(0, 0.3f);
            StartDialogue(interaction);
            PlayerController.Instance.DisableInputs();
        }
            
        if (PlayerController.Instance.characterInputs.UI.Interact.triggered)
        {
            DisplayNextSentence();
        }
    }


    private Queue<string> _sentences;
    
    private void OpenDialogue()
    {
        Cursor.visible = true;
        TargetCursor.instance.enabled = false;

        textBox.transform.DOLocalMoveY(1.5f, 0.8f);
        textBox.transform.DOScale(1, 0.8f);
    }
    
    public void StartDialogue(Interaction interaction)
    {
        PlayerController.Instance.enabled = false;
        OpenDialogue();

        _sentences = new Queue<string>();    
        _sentences.Clear();

        foreach (string sentence in interaction.sentences)          
        { 
            _sentences.Enqueue(sentence);
        }
        
        string sentences = _sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences));
    }
    
    IEnumerator TypeSentence (string sentence)
    {
        interactionTxt.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            interactionTxt.text += letter;
            yield return null;
        }
    }
    
    public void DisplayNextSentence()
    {
        if (_sentences.Count <= 0)
        {
            CloseInteraction();
            return;
        }
        
        string sentences = _sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences));
    }

    private void CloseInteraction()
    {
        PlayerController.Instance.enabled = true;
        Cursor.visible = true;
        TargetCursor.instance.enabled = true;
            
        textBox.transform.DOLocalMoveY(0f, 0.4f);
        textBox.transform.DOScale(0, 0.7f);
    }
}
