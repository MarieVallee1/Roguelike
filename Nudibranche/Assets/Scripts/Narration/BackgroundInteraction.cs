using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using DG.Tweening;
using Narration;
using TMPro;
using UnityEngine;

public class BackgroundInteraction : MonoBehaviour
{
    
    private bool _inZone;
    [SerializeField] private Interaction interaction;
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI interactionTxt;


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
