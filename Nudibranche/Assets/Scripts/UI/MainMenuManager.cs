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
        public Image blackScreen;
        [SerializeField] private GameObject mainMenu;
        private CanvasGroup _mainMenuCanvas;
        [SerializeField] private GameObject optionMenu;
        private CanvasGroup _optionMenuCanvas;

        [SerializeField] private GameObject optionButton;
        [SerializeField] private GameObject firstOptionSelected;
        [SerializeField] private GameObject optionReturnButton;

        private string _selectedButtons;
    
        [Header("States")]
        public bool mainMenuOpen;
        public bool optionMenuOpen;
    
    
        private void Awake()
        {
            _inputActions = new PlayerInputActions();
            _event = EventSystem.current;
        
            //set the alpha to 1
            blackScreen.color = new Color(0, 0, 0, 1);
        
            _mainMenuCanvas = mainMenu.GetComponent<CanvasGroup>();
            _optionMenuCanvas = optionMenu.GetComponent<CanvasGroup>();
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
            //Manages the first black screen
            blackScreen.DOFade(0, 2f).onComplete = DisableBlackScreen;

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
            //Fades to black before launching the game
            blackScreen.DOKill();
            blackScreen.enabled = true;
            blackScreen.DOFade(1, 3f).onComplete = LaunchGame;
            _inputActions.Disable();
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


        private void LaunchGame()
        {
            //Launches the game
            SceneManager.LoadScene("Scene_Terri");
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
        private void DisableBlackScreen()
        {
            blackScreen.enabled = false;
        }
    
    
    
        private void HandleSelectedButtons()
        {
            //Moves the cursor depending on the button selected
            switch (_event.currentSelectedGameObject.name)
            {
                case "PlayButton":
                {
                    cursor.DOAnchorPosY(0, 0.5f);
                }
                    break;
            
                case "OptionsButton":
                {
                    cursor.DOAnchorPosY(-80, 0.5f);
                }
                    break;
            
                case "QuitButton":
                {
                    cursor.DOAnchorPosY(-160, 0.5f);
                }
                    break;
            }
        }
    }
}
