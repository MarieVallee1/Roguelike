using System;
using System.Collections;
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
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private CanvasGroup choicesBox;
        [SerializeField] private CanvasGroup hearts;
        
        [Header("Images")]
        [SerializeField] private Image blackBackground;
        
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI dialogueTxt;
        [SerializeField] private TextMeshProUGUI choice1Txt;
        [SerializeField] private TextMeshProUGUI choice2Txt;
        
        [Header("Buttons")]
        [SerializeField] private GameObject choices;

        [Header("PnJ")] [SerializeField] private GameObject[] npc; 
        [SerializeField] private GameObject[] txtBox;

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
            PlayerController.Instance.DisableInputs();
            PlayerController.Instance.FreezeCharacter();
            
            _branchTaken = 0;
            OpenDialogue(dialogue);
            
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
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentences));
        }
        public void ContinueDialogue(Dialogue dialogue)
        {
            DisplayNextSentence(dialogue);
        }
        private void DisplayNextSentence(Dialogue dialogue)
        {
            if (_sentences.Count == 0 && _branchTaken == 0 && !dialogue.noChoiceDialogue)
            {
                OpenChoices();
                choice1Txt.text = dialogue.choices[0];
                choice2Txt.text = dialogue.choices[1];
                return;
            }

            if (_sentences.Count <= 0 && _branchTaken == 0 || _sentences1.Count <= 0 && _branchTaken == 1 || _sentences2.Count <= 0 && _branchTaken == 2)
            {
                EndDialogue(dialogue);
                return;
            }

            switch (_branchTaken)
            {
                case 0 :
                {
                    string sentences = _sentences.Dequeue();
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(sentences));
                }
                    
                    break;
                case 1 :
                {
                    string sentences = _sentences1.Dequeue();
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(sentences));
                }
                    break;
                case 2 :
                {
                    string sentences = _sentences2.Dequeue();
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(sentences));
                }
                    break;
            }
        }
        IEnumerator TypeSentence (string sentence)
        {
            dialogueTxt.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueTxt.text += letter;
                yield return null;
            }
        }
        void EndDialogue(Dialogue dialogue)
        {
            if (_branchTaken == 1)
            {
                switch (dialogue.skillIndex)
                {
                    case 0:
                    {
                        PlayerController.Instance.skillIndex = PlayerController.Instance.skillIndex = dialogue.skillIndex;
                    }
                        break;
                    case 1 :
                    {
                        PlayerController.Instance.skillIndex = PlayerController.Instance.skillIndex = dialogue.skillIndex;
                    }
                        break;
                    case 2 :
                    {
                        PlayerController.Instance.skillIndex = PlayerController.Instance.skillIndex = dialogue.skillIndex;
                    }
                        break;
                }
                
                UIManager.instance.UpdateSkillInfo();
            }

            CloseDialogue(dialogue);
            
            PlayerController.Instance.EnableInputs();
            PlayerController.Instance.UnfreezeCharacter();
        }
        
        
        private void OpenDialogue(Dialogue dialogue)
        {
            switch (dialogue.skillIndex)
            {
                case 0 :
                    npc[0].SetActive(true);
                    npc[1].SetActive(false);
                    npc[2].SetActive(false); 
                    txtBox[3].SetActive(false); 
                    
                    txtBox[0].SetActive(true);
                    txtBox[1].SetActive(false);
                    txtBox[2].SetActive(false); 
                    txtBox[3].SetActive(false); 
                    break;
                
                case 1: 
                    npc[0].SetActive(false);
                    npc[1].SetActive(true);
                    npc[2].SetActive(false); 
                    txtBox[3].SetActive(false); 
                    
                    txtBox[0].SetActive(false);
                    txtBox[1].SetActive(true);
                    txtBox[2].SetActive(false); 
                    txtBox[3].SetActive(false); 
                    break;
                
                case 2: 
                    npc[0].SetActive(false);
                    npc[1].SetActive(false);
                    npc[2].SetActive(true); 
                    npc[3].SetActive(false); 
                    
                    txtBox[0].SetActive(false);
                    txtBox[1].SetActive(false);
                    txtBox[2].SetActive(true); 
                    txtBox[3].SetActive(false); 
                    break;
                
                case 3 :
                    npc[0].SetActive(false);
                    npc[1].SetActive(false);
                    npc[2].SetActive(false); 
                    npc[3].SetActive(true); 
                    
                    txtBox[0].SetActive(false);
                    txtBox[1].SetActive(false);
                    txtBox[2].SetActive(false); 
                    txtBox[3].SetActive(true); 
                    break;
                
                default: print("no skill index read");
                    break;
            }
            
            Cursor.visible = true;
            TargetCursor.instance.enabled = false;
            
            hearts.DOFade(0, 0.8f);
            
            dialogueBox.transform.DOLocalMoveY(0, 0.8f);

            blackBackground.DOFade(0.5f, 1f);
        }
        private void CloseDialogue(Dialogue dialogue)
        {
            Cursor.visible = true;
            TargetCursor.instance.enabled = true;
            
            dialogueBox.transform.DOLocalMoveY(-1100, 0.8f);

            blackBackground.DOFade(0.5f, 1f).endValue = new Color(0,0,0,0);
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
