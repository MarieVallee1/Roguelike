using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using DG.Tweening;
using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    //Audio
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private List<GameObject> portraits;
    [SerializeField] private List<GameObject> portraitsOver;
    [SerializeField] private List<GameObject> portraitsCooldown;
    [SerializeField] private List<Image> mouleScore;
    [SerializeField] private List<Image> crevetteScore;
    [SerializeField] private List<Image> cannonierScore;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject cheatButton;
    [SerializeField] private GameObject optionButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject destinationHeader;
    [SerializeField] private GameObject moneyButton;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject invulnerabilityButton;
    [SerializeField] private CanvasGroup pauseButtons;
    [SerializeField] private GameObject pauseButtonGroup;
    [SerializeField] private CanvasGroup optionButtons;
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private GameObject cheatMenu;
    [SerializeField] private CanvasGroup cheatButtons;
    [SerializeField] private CanvasGroup deathScreenAlpha;

    [SerializeField] private Animation pearlPanelAnim;
    [SerializeField] private ParticleSystem pearlPanelVFX;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private TextMeshProUGUI objectInfo;
    [SerializeField] private ParticleSystem slashEffects;
    [SerializeField] private Animator blackScreen;
    
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider mainVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    private EventSystem _event;
    private static readonly int FadeIt = Animator.StringToHash("FadeIt");


    public bool pauseMenuOn;
    public bool optionMenuOn;
    public bool cheatMenuOn;

    private const string MIXER_MAIN = "MainVolume";
    private const string MIXER_MUSIC = "MusicVolume";
    private const string MIXER_SFX = "SFXVolume";
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;
        
        _event = EventSystem.current;

        mainVolumeSlider.value = PlayerPrefs.GetFloat("MainVolume");
        if (mainVolumeSlider.value < 0.01f) mainVolumeSlider.value = 1;
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        if (musicVolumeSlider.value < 0.01f) musicVolumeSlider.value = 1;
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        if (sfxVolumeSlider.value < 0.01f) sfxVolumeSlider.value = 1;
        
        DOTween.KillAll();
        
        mainVolumeSlider.onValueChanged.AddListener(SetMainVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
    }
    
    void Start()
    {
        UpdateSkillInfo();
        pauseMenuOn = false;
        
        mixer.SetFloat(MIXER_MAIN, Mathf.Log10(PlayerPrefs.GetFloat("MainVolume"))*20);
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume"))*20);
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume"))*20);
    }

    private void Update()
    {
        if (!PlayerController.Instance.isDead)
        {
            if(PlayerController.Instance.characterInputs.Character.Pause.triggered && !pauseMenuOn) OpenPauseMenu();
            if(PlayerController.Instance.characterInputs.UI.Escape.triggered && pauseMenuOn && !cheatMenuOn) ClosePauseMenu();
            if(PlayerController.Instance.characterInputs.UI.Escape.triggered && cheatMenuOn) CloseCheatMenuButton();
            if(PlayerController.Instance.characterInputs.UI.Escape.triggered && optionMenuOn) CloseOptionsMenu();
            if(pauseMenuOn)HandleSelectedButtons();
        }
        
    }
    
    
    public void UpdateUiPearl()
    {
        GameManager.instance.pearlAmountText.text = GameManager.instance.pearlAmount + "";
    } 
    public void PearlUpFeedback()
    {
        pearlPanelAnim.Play();
        pearlPanelVFX.Play();
    }
    
    public void UpdateSkillInfo()
    {
        switch (PlayerController.Instance.skillIndex)
        {
            case 0: 
                portraits[0].SetActive(true);
                portraitsCooldown[0].SetActive(true);
                portraitsOver[0].SetActive(true);
                portraits[1].SetActive(false);
                portraitsCooldown[1].SetActive(false);
                portraitsOver[1].SetActive(false);
                portraits[2].SetActive(false);
                portraitsCooldown[2].SetActive(false);
                portraitsOver[2].SetActive(false);
                break;
            
            case 1: 
                portraits[0].SetActive(false);
                portraitsCooldown[0].SetActive(false);
                portraitsOver[0].SetActive(false);
                portraits[1].SetActive(true);
                portraitsCooldown[1].SetActive(true);
                portraitsOver[1].SetActive(true);
                portraits[2].SetActive(false);
                portraitsCooldown[2].SetActive(false);
                portraitsOver[2].SetActive(false);
                break;
            
            case 2:
                portraits[0].SetActive(false);
                portraitsCooldown[0].SetActive(false);
                portraitsOver[0].SetActive(false);
                portraits[1].SetActive(false);
                portraitsCooldown[1].SetActive(false);
                portraitsOver[1].SetActive(false);
                portraits[2].SetActive(true);
                portraitsCooldown[2].SetActive(true);
                portraitsOver[2].SetActive(true);
                break;
            
            default:
                portraits[0].SetActive(false);
                portraitsCooldown[0].SetActive(false);
                portraitsOver[0].SetActive(false);
                portraits[1].SetActive(false);
                portraitsCooldown[1].SetActive(false);
                portraitsOver[1].SetActive(false);
                portraits[2].SetActive(false);
                portraitsCooldown[2].SetActive(false);
                portraitsOver[2].SetActive(false);
                break;
        }
    }

    public void BlackScreenFadeIn()
    {
        blackScreen.SetBool("Faded", false);
    } 
    public void BlackScreenFadeOut()
    {
        blackScreen.SetBool("Faded", true);
    } 

    public IEnumerator ScieRanoSlash()
    {
        BlackScreenFadeOut();
        yield return new WaitForSeconds(0.5f);
        slashEffects.Play();
        yield return new WaitForSeconds(1f);
        slashEffects.Stop();
        BlackScreenFadeIn();
    }
    
    private void OpenPauseMenu()
    {
        Time.timeScale = 0;
        _event.SetSelectedGameObject(continueButton);
        pauseMenuOn = true;
        pauseMenu.SetActive(true);
        PlayerController.Instance.DisableInputs();
        Cursor.visible = true;
        TargetCursor.instance.enabled = false;
    }
    public void ClosePauseMenu()
    {
        PlayClickAudio();
        
        Time.timeScale = 1;
        pauseMenuOn = false;
        pauseMenu.SetActive(false);
        PlayerController.Instance.EnableInputs();
        Cursor.visible = false;
        TargetCursor.instance.enabled = true;
    }

    public void ReturnToMainMenu()
    {
        PlayClickAudio();
        AudioList.Instance.StartMusic(AudioList.Music.menu,true);
        SceneManager.LoadScene("Scene_MainMenu");
    }
    
    public void CheatButton()
    {
        PlayClickAudio();
        
        //Fades one menu to let the other appear
        //pauseButtons.DOFade(0, 0.5f).onComplete = OpenCheat;
        OpenCheat();
    }
    
    public void OptionsButton()
    {
        PlayClickAudio();
        
        //Fades one menu to let the other appear
        //pauseButtons.DOFade(0, 0.5f).onComplete = OpenCheat;
        OpenOptions();
    }

    public void OpenDeathScreen()
    {
        deathScreen.SetActive(true);
        deathScreenAlpha.DOFade(1, 1f);
        StartCoroutine(ScoreScreenCalculation());
    }

    private IEnumerator ScoreScreenCalculation()
    {
        //Allow the enemies to appear on the score screen
        
        if (GameManager.instance.mouleKilled <= mouleScore.Count)
        {
            for (int i = 0; i < GameManager.instance.mouleKilled; i++)
            {
                mouleScore[i].enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            for (int i = 0; i < mouleScore.Count; i++)
            {
                mouleScore[i].enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        if (GameManager.instance.crevetteKilled <= crevetteScore.Count)
        {
            for (int i = 0; i < GameManager.instance.crevetteKilled; i++)
            {
                crevetteScore[i].enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            for (int i = 0; i < crevetteScore.Count; i++)
            {
                crevetteScore[i].enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        if (GameManager.instance.cannonierKilled <= cannonierScore.Count)
        {
            for (int i = 0; i < GameManager.instance.cannonierKilled; i++)
            {
                cannonierScore[i].enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            for (int i = 0; i < cannonierScore.Count; i++)
            {
                cannonierScore[i].enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
        }

    }
    
    public void CloseDeathScreen()
    {
        deathScreenAlpha.DOFade(0, 1f);
        deathScreen.SetActive(true);
    }
    private void OpenCheat()
    {
        pauseButtonGroup.SetActive(false);
        cheatMenuOn = true;
        cheatMenu.SetActive(true);
        cheatButtons.alpha = 1;
        //cheatButtons.DOFade(1, 0.5f);
    }

    private void CloseCheatMenuButton()
    {
        PlayClickAudio();
        
        //cheatButtons.DOFade(0, 0.5f).onComplete = CloseCheatMenu();
        cheatButtons.alpha = 0;
        CloseCheatMenu();
    }

    private void CloseCheatMenu()
    {
        PlayClickAudio();
        
        cheatMenu.SetActive(false);
        cheatMenuOn = false;
        pauseButtonGroup.SetActive(true);
        pauseButtons.alpha = 1;
        //pauseButtons.DOFade(1, 0.5f);
    }
    
    private void OpenOptions()
    {
        pauseButtonGroup.SetActive(false);
        optionMenuOn = true;
        optionMenu.SetActive(true);
        optionButtons.alpha = 1;
    }
    
    public void CloseOptionsMenu()
    {
        PlayClickAudio();
        
        optionMenu.SetActive(false);
        optionMenuOn = false;
        pauseButtonGroup.SetActive(true);
        pauseButtons.alpha = 1;
        //pauseButtons.DOFade(1, 0.5f);
    }
    
    private void HandleSelectedButtons()
    {
        //Moves the cursor depending on the button selected
        switch (_event.currentSelectedGameObject.name)
        {
            case "ContinueButton":
            {
                cursor.DOAnchorPosY(129, 0.5f).SetUpdate(true);
            }
                break;
            
            case "OptionsButton":
            {
                cursor.DOAnchorPosY(28, 0.5f).SetUpdate(true);
            }
                break;
            
            case "QuitButton":
            {
                cursor.DOAnchorPosY(-32, 0.5f).SetUpdate(true);
            }
                break;
            
            case "CheatButton":
            {
                cursor.DOAnchorPosY(90, 0.5f).SetUpdate(true);
            }
                break;
            case "Destination":
            {
                cursor.DOAnchorPosY(109, 0.5f).SetUpdate(true);
            }
                break;
            case "Money":
            {
                cursor.DOAnchorPosY(-11, 0.5f).SetUpdate(true);
            }
                break;
            case "Invulnerability":
            {
                cursor.DOAnchorPosY(-50, 0.5f).SetUpdate(true);
            }
                break;
        }
    }
    
    public void PlayButtonIsSelected()
    {
        _event.SetSelectedGameObject(continueButton);
    }
    public void CheatButtonIsSelected()
    {
        _event.SetSelectedGameObject(cheatButton);
    }
    public void OptionButtonIsSelected()
    {
        _event.SetSelectedGameObject(optionButton);
    }
    public void QuitButtonIsSelected()
    {
        _event.SetSelectedGameObject(quitButton);
    }
    public void DestinationButtonIsSelected()
    {
        _event.SetSelectedGameObject(destinationHeader);
    }
    public void MoneyButtonIsSelected()
    {
        _event.SetSelectedGameObject(moneyButton);
    }
    public void InvulnerabilityButtonIsSelected()
    {
        _event.SetSelectedGameObject(invulnerabilityButton);
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
