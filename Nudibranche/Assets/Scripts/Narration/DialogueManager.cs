using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Narration
{
    public class DialogueManager : MonoBehaviour
    {
        
        [SerializeField] private GameObject dialogueBox1;
        [SerializeField] private GameObject dialogueBox2;
        [SerializeField] private Image spriteNpc;
        [SerializeField] private TextMeshProUGUI nameTxt;
        [SerializeField] private TextMeshProUGUI dialogueTxt;

        public static DialogueManager instance;
        private Queue<string> _sentences;

        private bool _dialogueStarted;

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
        }

        public void StartDialogue(Dialogue dialogue)
        {
            OpenDialogue();  

            if (!_dialogueStarted)
            {
                nameTxt.text = dialogue.name;                            
                                                           
                _sentences.Clear();                                      
                                                           
                foreach (string sentence in dialogue.sentences)          
                {                                                        
                    _sentences.Enqueue(sentence);                        
                }                                                        
                
                _dialogueStarted = true;                                  
            }
            DisplayNextSentence();      
        }

        public void DisplayNextSentence()
        {
            if (_sentences.Count <= 0)
            {
                EndDialogue();
                return;
            }

            string sentence = _sentences.Dequeue();
            dialogueTxt.text = sentence;
        }

        void EndDialogue()
        {
            CloseDialogue();

            _dialogueStarted = false;
        }

        private void OpenDialogue()
        {
            dialogueBox1.transform.DOLocalMoveX(0, 0.8f);
            dialogueBox2.transform.DOLocalMoveX(0, 0.8f);
            spriteNpc.DOFade(1, 1f);
        }
        
        private void CloseDialogue()
        {
            dialogueBox1.transform.DOLocalMoveX(2000, 0.8f);
            dialogueBox2.transform.DOLocalMoveX(-2000, 0.8f);
            spriteNpc.DOFade(0, 0.8f);
        }                                       
    }
}
