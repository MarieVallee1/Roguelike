using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using DG.Tweening;
using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private List<GameObject> portraits;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject optionButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject firstOptionSelected;
    [SerializeField] private CanvasGroup pauseButtons;
    [SerializeField] private GameObject pauseButtonGroup;
    [SerializeField] private CanvasGroup optionButtons;
    [SerializeField] private GameObject optionMenu;

    [SerializeField] private GameObject objectPanel;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private TextMeshProUGUI objectInfo;
    [SerializeField] private ParticleSystem slashEffects;
    [SerializeField] private Animator blackScreen;
    private EventSystem _event;
    private static readonly int FadeIt = Animator.StringToHash("FadeIt");
    

    public bool pauseMenuOn;
    public bool optionMenuOn;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;
        
        _event = EventSystem.current;
        
        DOTween.KillAll();
    }
    
    void Start()
    {
        UpdateSkillInfo();
        pauseMenuOn = false;
    }

    private void Update()
    {
        if(PlayerController.Instance.characterInputs.Character.Pause.triggered && !pauseMenuOn) OpenPauseMenu();
        if(PlayerController.Instance.characterInputs.UI.Escape.triggered && pauseMenuOn) ClosePauseMenu();
        if(pauseMenuOn)HandleSelectedButtons();
    }

    public void UpdateSkillInfo()
    {
        switch (PlayerController.Instance.skillIndex)
        {
            case 0: 
                portraits[0].SetActive(true);
                portraits[1].SetActive(false);
                portraits[2].SetActive(false);
                break;
            
            case 1: 
                portraits[0].SetActive(false);
                portraits[1].SetActive(true);
                portraits[2].SetActive(false);
                break;
            
            case 2:
                portraits[0].SetActive(false);
                portraits[1].SetActive(false);
                portraits[2].SetActive(true);
                break;
            
            default:
                portraits[0].SetActive(false);
                portraits[1].SetActive(false);
                portraits[2].SetActive(false);
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
        Time.timeScale = 1;
        pauseMenuOn = false;
        pauseMenu.SetActive(false);
        PlayerController.Instance.EnableInputs();
        Cursor.visible = false;
        TargetCursor.instance.enabled = true;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Scene_MainMenu");
    }
    
    public void OpenOptionsMenu()
    {
        //Fades one menu to let the other appear
        pauseButtons.DOFade(0, 0.5f).onComplete = DisablePauseMenu;
        EnableOptionMenu();
        optionButtons.DOFade(1, 0.5f);

        _event.SetSelectedGameObject(firstOptionSelected);
    }
    
    private void HandleSelectedButtons()
    {
        //Moves the cursor depending on the button selected
        switch (_event.currentSelectedGameObject.name)
        {
            case "ContinueButton":
            {
                cursor.DOAnchorPosY(137, 0.5f).SetUpdate(true);
            }
                break;
            
            case "OptionsButton":
            {
                cursor.DOAnchorPosY(57, 0.5f).SetUpdate(true);
            }
                break;
            
            case "QuitButton":
            {
                cursor.DOAnchorPosY(-23, 0.5f).SetUpdate(true);
            }
                break;
        }
    }
    
    public void PlayButtonµIsSelected()
    {
        _event.SetSelectedGameObject(continueButton);
    } 
        
    public void OptionButtonµIsSelected()
    {
        _event.SetSelectedGameObject(optionButton);
    } 
        
    public void QuitButtonµIsSelected()
    {
        _event.SetSelectedGameObject(quitButton);
    }

    private void DisablePauseMenu()
    {
        pauseButtonGroup.SetActive(false);
    }
    private void EnablePauseMenu()
    {
        pauseButtonGroup.SetActive(true);
    }
    private void EnableOptionMenu()
    {
        optionMenu.SetActive(true);
        optionMenuOn = true;
    }
    private void DisableOptionMenu()
    {
        optionMenu.SetActive(false);
        optionMenuOn = false;
    }
}
