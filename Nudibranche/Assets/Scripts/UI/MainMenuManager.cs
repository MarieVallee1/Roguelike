using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuManager : MonoBehaviour
    {
        private PlayerInputActions _inputActions;
        private EventSystem _event;

        [Header("Menu References")]
        [SerializeField] private RectTransform cursor;
        [SerializeField] private GameObject mainMenu;
        private CanvasGroup _mainMenuCanvas;
        [SerializeField] private GameObject optionMenu;
        private CanvasGroup _optionMenuCanvas;

        [SerializeField] private GameObject optionButton;
        [SerializeField] private GameObject playButton;
        [SerializeField] private GameObject quitButton;
        [SerializeField] private GameObject firstOptionSelected;
        [SerializeField] private GameObject optionReturnButton;

        [SerializeField] private Animator blackScreenAnim;
        
        private string _selectedButtons;
    
        [Header("States")]
        public bool mainMenuOpen;
        public bool optionMenuOpen;
    
    
        private void Awake()
        {
            Time.timeScale = 1;
            
            _inputActions = new PlayerInputActions();
            _event = EventSystem.current;

            DOTween.KillAll();
            _mainMenuCanvas = mainMenu.GetComponent<CanvasGroup>();
            _optionMenuCanvas = optionMenu.GetComponent<CanvasGroup>();
            
            //Manages the first black screen
            blackScreenAnim.SetBool("Faded", false);
        }
        private void OnEnable()
        {
            //Activates the UI Inputs
            _inputActions.Character.Disable();
            _inputActions.UI.Enable();

            _inputActions.UI.Interact.performed += ctx =>
            {
                if(_event.currentSelectedGameObject == optionButton) OpenOptionsButton();
                else if(_event.currentSelectedGameObject == optionReturnButton)QuitOptionsButton();
                
                //Moves the cursor depending on the button selected
                switch (_event.currentSelectedGameObject.name)
                {
                    case "PlayButton":
                    {
                        StartButton();
                    }
                        break;
            
                    case "OptionsButton":
                    {
                        OpenOptionsButton();
                    }
                        break;
            
                    case "QuitButton":
                    {
                        QuitButton();
                    }
                        break;
                    
                    case "ReturnButton":
                    {
                        QuitOptionsButton();
                    }
                        break;
                }
            };
        }
        void Start()
        {
            EnableButtonFunction();

            mainMenuOpen = true;
            optionMenuOpen = false;
        }
        private void Update()
        {
            //Manages the cursor in the main menu
            if(mainMenuOpen) HandleSelectedButtons();
        }
        
        

        public void StartButton()
        {
            DisableButtonFunction();
            
            //Fades to black before launching the game
            blackScreenAnim.SetBool("Faded", true);

            _inputActions.Disable();
            
            StartCoroutine(LaunchGame());
        }
        public void OpenOptionsButton()
        {
            //Fades one menu to let the other appear
            _mainMenuCanvas.DOFade(0, 0.5f).onComplete = DisableMainMenu;
            optionMenu.SetActive(true);
            _optionMenuCanvas.DOFade(1, 0.5f);
            optionMenuOpen = true;
            
            _event.SetSelectedGameObject(firstOptionSelected);
        }
        public void QuitOptionsButton()
        {
            //Fades one menu to let the other appear
            _optionMenuCanvas.DOFade(0, 0.5f).onComplete = DisableOptionsMenu;
            mainMenu.SetActive(true);
            _mainMenuCanvas.DOFade(1, 0.5f);
            mainMenuOpen = true;

            _event.SetSelectedGameObject(optionButton);
        }
        public void QuitButton()
        {
            //Quits the game
            Application.Quit();
        }


        private IEnumerator LaunchGame()
        {
            yield return new WaitForSeconds(1f);
            //Launches the game
            SceneManager.LoadScene("Scene_Main");
        }
        private void DisableMainMenu()
        {
            mainMenu.SetActive(false);
            mainMenuOpen = false;
        }
        private void DisableOptionsMenu()
        {
            optionMenu.SetActive(false);
            optionMenuOpen = false;
        }
     
        

        private void HandleSelectedButtons()
        {
            if (_event.currentSelectedGameObject != null)
            {
                //Moves the cursor depending on the button selected
                switch (_event.currentSelectedGameObject.name)
                {
                    case "PlayButton":
                    {
                        cursor.DOKill();
                        cursor.DOAnchorPosY(279, 0.5f);
                    }
                        break;
            
                    case "OptionsButton":
                    {
                        cursor.DOKill();
                        cursor.DOAnchorPosY(205, 0.5f);
                    }
                        break;
            
                    case "QuitButton":
                    {
                        cursor.DOKill();
                        cursor.DOAnchorPosY(126, 0.5f);
                    }
                        break;
                
                    default: print("Nothing Selected");
                        break;
                }
            }
            
        }

        public void PlayButtonµIsSelected()
        {
            _event.SetSelectedGameObject(playButton);
        } 
        
        public void OptionButtonµIsSelected()
        {
            _event.SetSelectedGameObject(optionButton);
        } 
        
        public void QuitButtonµIsSelected()
        {
            _event.SetSelectedGameObject(quitButton);
        }

        private void DisableButtonFunction()
        {
            playButton.GetComponent<Button>().interactable = false;
            optionButton.GetComponent<Button>().interactable = false;
            quitButton.GetComponent<Button>().interactable = false;
        }
        
        private void EnableButtonFunction()
        {
            playButton.GetComponent<Button>().interactable = true;
            optionButton.GetComponent<Button>().interactable = true;
            quitButton.GetComponent<Button>().interactable = true;
        }
    }
}
