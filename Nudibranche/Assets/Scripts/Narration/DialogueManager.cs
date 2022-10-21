using System;
using System.Collections.Generic;
using Character;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Narration
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;
        
        [Header("Boxes")]
        [SerializeField] private GameObject dialogueBox1;
        [SerializeField] private GameObject dialogueBox2;
        [SerializeField] private CanvasGroup choicesBox;
        [SerializeField] private CanvasGroup hearts;
        
        [Header("Images")]
        [SerializeField] private Image spriteNpc;
        [SerializeField] private Image blackBackground;
        
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI nameTxt;
        [SerializeField] private TextMeshProUGUI dialogueTxt;
        [SerializeField] private TextMeshProUGUI choice1Txt;
        [SerializeField] private TextMeshProUGUI choice2Txt;
        
        [Header("Buttons")]
        [SerializeField] private GameObject choices;


        private Queue<string> _sentences;
        private Queue<string> _sentences1;
        private Queue<string> _sentences2;

        private int _branchTaken;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }

            instance = this;
        }

        private void Start()
        {
            _sentences = new Queue<string>();
            _sentences1 = new Queue<string>();
            _sentences2 = new Queue<string>();
        }

        public void StartDialogue(Dialogue dialogue)
        {
            _branchTaken = 0;
            PlayerController.instance.DisableInputs();
            OpenDialogue();
            
            nameTxt.text = dialogue.name;     
            
            _sentences = new Queue<string>();    
            _sentences.Clear();
            _sentences1 = new Queue<string>();    
            _sentences1.Clear();
            _sentences2 = new Queue<string>();    
            _sentences2.Clear();
            
            foreach (string sentence in dialogue.sentences)          
            { 
                _sentences.Enqueue(sentence);
            }
            
            foreach (string sentence1 in dialogue.choice1Branch)          
            { 
                _sentences1.Enqueue(sentence1);
            }
            
            foreach (string sentence2 in dialogue.choice2Branch)          
            { 
                _sentences2.Enqueue(sentence2);
            }
            
            string sentences = _sentences.Dequeue();
            dialogueTxt.text = sentences;
        }

        public void ContinueDialogue(Dialogue dialogue)
        {
            DisplayNextSentence(dialogue);
        }

        private void DisplayNextSentence(Dialogue dialogue)
        {
            if (_sentences.Count == 0 && _branchTaken == 0)
            {
                OpenChoices();
                choice1Txt.text = dialogue.choices[0];
                choice2Txt.text = dialogue.choices[1];
                return;
                
            }

            if (_sentences.Count <= 0 && _branchTaken == 0 || _sentences1.Count <= 0 || _sentences2.Count <= 0)
            {
                EndDialogue();
                return;
            }

            switch (_branchTaken)
            {
                case 0 :
                {
                    string sentences = _sentences.Dequeue();
                    dialogueTxt.text = sentences;
                }
                    
                    break;
                case 1 :
                {
                    string sentences = _sentences1.Dequeue();
                    dialogueTxt.text = sentences;
                }
                    break;
                case 2 :
                {
                    string sentences = _sentences2.Dequeue();
                    dialogueTxt.text = sentences;
                }
                    break;
            }
        }

        void EndDialogue()
        {
            CloseDialogue();
            PlayerController.instance.EnableInputs();
        }

        private void OpenDialogue()
        {
            Cursor.visible = true;
            TargetCursor.instance.enabled = false;
            
            hearts.DOFade(0, 0.8f);
            
            dialogueBox1.transform.DOLocalMoveX(0, 0.8f);
            dialogueBox2.transform.DOLocalMoveX(0, 0.8f);
            
            blackBackground.DOFade(0.5f, 1f);
            spriteNpc.DOFade(1, 1f);
        }
        private void CloseDialogue()
        {
            Cursor.visible = true;
            TargetCursor.instance.enabled = false;
            
            dialogueBox1.transform.DOLocalMoveX(2000, 0.8f);
            dialogueBox2.transform.DOLocalMoveX(-2000, 0.8f);
            
            blackBackground.DOFade(0.5f, 1f).endValue = new Color(0,0,0,0);
            spriteNpc.DOFade(0, 0.8f).endValue = new Color(0,0,0,0);
            hearts.DOFade(1, 0.8f);
        }

        private void OpenChoices()
        {
            choices.SetActive(true);
            choicesBox.DOFade(1f, 1f);
        }
        
        public void Choices1()
        {
            _branchTaken = 1;
            choicesBox.DOFade(0f, 1f);
            choices.SetActive(false);

            string sentence = _sentences1.Dequeue();
            dialogueTxt.text = sentence;
        }
        
        public void Choices2()
        {
            _branchTaken = 2;
            choicesBox.DOFade(0f, 1f);
            choices.SetActive(false);

            string sentence = _sentences2.Dequeue();
            dialogueTxt.text = sentence;
        }
    }
}
