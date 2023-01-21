using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
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
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private RectTransform cursor;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private CanvasGroup mainMenuCanvas;
        [SerializeField] private GameObject optionMenu;
        [SerializeField] private CanvasGroup optionMenuCanvas;
        [SerializeField] private GameObject scoreMenu;
        [SerializeField] private CanvasGroup scoreMenuCanvas;

        [SerializeField] private GameObject optionButton;
        [SerializeField] private GameObject playButton;
        [SerializeField] private GameObject scoreButton;
        [SerializeField] private GameObject quitButton;
        [SerializeField] private GameObject firstOptionSelected;

        [SerializeField] private AudioMixer mixer;
        [SerializeField] private Slider mainVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        private const string MIXER_MAIN = "MainVolume";
        private const string MIXER_MUSIC = "MusicVolume";
        private const string MIXER_SFX = "SFXVolume";
        
        [SerializeField] private Animator blackScreenAnim;

        private VideoManager _videoManager;
        
        private string _selectedButtons;
    
        [Header("States")]
        public bool mainMenuOpen;
        public bool optionMenuOpen;
        public bool scoreMenuOpen;
    
    
        private void Awake()
        {
            Time.timeScale = 1;
            
            _inputActions = new PlayerInputActions();
            _event = EventSystem.current;

            mainVolumeSlider.value = PlayerPrefs.GetFloat("MainVolume");
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");

            DOTween.KillAll();

            mainVolumeSlider.onValueChanged.AddListener(SetMainVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
            
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
                    
                    case "ScoreButton":
                    {
                        OpenScoreButton();
                    }
                        break;
                }
            };
        }
        void Start()
        {
            mixer.SetFloat(MIXER_MAIN, Mathf.Log10(PlayerPrefs.GetFloat("MainVolume"))*20);
            mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume"))*20);
            mixer.SetFloat(MIXER_SFX, Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume"))*20);
            
            _videoManager = VideoManager.instance;
            
            EnableButtonFunction();

            mainMenuOpen = true;
            optionMenuOpen = false;
            scoreMenuOpen = false;
            
            ScoreManager.instance.ScoreboardUI();
        }
        private void Update()
        {
            //Manages the cursor in the main menu
            if(mainMenuOpen) HandleSelectedButtons();
        }
        
        

        public void StartButton()
        {
            AudioList.Instance.cinematicPlayed = false;
            
            PlayClickAudio();
            
            DisableButtonFunction();
            
           _videoManager.PlayVideo(0,0,0);

           _inputActions.Disable();
            
            StartCoroutine(LaunchGame());
        }
        public void OpenOptionsButton()
        {
            PlayClickAudio();
            
            //Fades one menu to let the other appear
            mainMenuCanvas.DOFade(0, 0.5f).onComplete = DisableMainMenu;
            optionMenu.SetActive(true);
            optionMenuCanvas.DOFade(1, 0.5f);
            optionMenuOpen = true;
            
            _event.SetSelectedGameObject(firstOptionSelected);
        }
        
        public void OpenScoreButton()
        {
            
            DOTween.KillAll();
            
            PlayClickAudio();
            
            //Fades one menu to let the other appear
            mainMenuCanvas.DOFade(0, 0.5f).onComplete = DisableMainMenu;
            scoreMenu.SetActive(true);
            scoreMenuCanvas.DOFade(1, 0.5f);
            scoreMenuOpen = true;
        }
        public void QuitButton()
        {
            PlayClickAudio();
            
            //Quits the game
            Application.Quit();
        }


        private IEnumerator LaunchGame()
        {
            yield return new WaitForSeconds(3f);
            
            //Fades to black before launching the game
            //blackScreenAnim.SetBool("Faded", true);
            
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
        private void DisableScoreMenu()
        {
            scoreMenu.SetActive(false);
            scoreMenuOpen = false;
        }

        public void CloseScoreMenu()
        {
            PlayClickAudio();
            
            //Fades one menu to let the other appear
            scoreMenuCanvas.DOFade(0, 0.5f).onComplete = DisableScoreMenu;
            mainMenu.SetActive(true);
            mainMenuCanvas.DOFade(1, 0.5f);
            mainMenuOpen = true;
        }
        
        public void CloseOptionMenu()
        {
            PlayClickAudio();
            
            //Fades one menu to let the other appear
            optionMenuCanvas.DOFade(0, 0.5f).onComplete = DisableOptionsMenu;
            mainMenu.SetActive(true);
            mainMenuCanvas.DOFade(1, 0.5f);
            mainMenuOpen = true;
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
                        cursor.DOAnchorPosY(300, 0.5f);
                    }
                        break;
            
                    case "OptionsButton":
                    {
                        cursor.DOKill();
                        cursor.DOAnchorPosY(235, 0.5f);
                    }
                        break;
            
                    case "ScoreButton":
                    {
                        cursor.DOKill();
                        cursor.DOAnchorPosY(164, 0.5f);
                    }
                        break;
                    
                    case "QuitButton":
                    {
                        cursor.DOKill();
                        cursor.DOAnchorPosY(118, 0.5f);
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
        public void ScoreButtonµIsSelected()
        {
            _event.SetSelectedGameObject(scoreButton);
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
            scoreButton.GetComponent<Button>().interactable = false;
        }
        private void EnableButtonFunction()
        {
            playButton.GetComponent<Button>().interactable = true;
            optionButton.GetComponent<Button>().interactable = true;
            scoreButton.GetComponent<Button>().interactable = true;
            quitButton.GetComponent<Button>().interactable = true;
        }


        public void Fullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        //Audio
        private void PlayClickAudio()
        {
            audioSource.PlayOneShot(AudioList.Instance.uiClick);
        }
        
        private void SetMainVolume(float value)
        {
            mixer.SetFloat(MIXER_MAIN, Mathf.Log10(value)*20);
            PlayerPrefs.SetFloat("MainVolume", value);
        }
        private void SetMusicVolume(float value)
        {
            mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value)*20);
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
        private void SetSfxVolume(float value)
        {
            mixer.SetFloat(MIXER_SFX, Mathf.Log10(value)*20);
            PlayerPrefs.SetFloat("SFXVolume", value);
        }
    }
}
