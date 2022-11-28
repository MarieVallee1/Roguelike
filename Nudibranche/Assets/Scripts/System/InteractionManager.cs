using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using DG.Tweening;
using Narration;
using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance;

    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI interactionTxt;

    private Queue<string> _sentences;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;
    }
    
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
