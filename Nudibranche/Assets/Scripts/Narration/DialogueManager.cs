using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Narration
{
    public class DialogueManager : MonoBehaviour
    {
        
        [SerializeField] private GameObject dialogueBox;
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

            dialogueBox.SetActive(false);
        }

        public void StartDialogue(Dialogue dialogue)
        {
            dialogueBox.SetActive(true);    

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
            dialogueBox.SetActive(false);    

            _dialogueStarted = false;
        }
    }
}
