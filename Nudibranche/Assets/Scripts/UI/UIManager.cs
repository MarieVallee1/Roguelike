using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using DG.Tweening;
using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private List<GameObject> portraits;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private TextMeshProUGUI objectInfo;
    [SerializeField] private ParticleSystem slashEffects;
    [SerializeField] private Animator blackScreen;
    private static readonly int FadeIt = Animator.StringToHash("FadeIt");


    public bool pauseMenuOn;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;
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
    public void UpdateObjectInfo()
    {
        //Displays the object index if the player has an object
        if (ItemManager.Instance.lastConsumable < 0)
        {
            objectInfo.text = "Null";
        }
        else
        {
            objectInfo.text = "" + ItemManager.Instance.ConsumableInfo().stats.objectName;
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
        blackScreen.SetTrigger(FadeIt);
        yield return new WaitForSeconds(0.5f);
        slashEffects.Play();
        yield return new WaitForSeconds(1f);
        slashEffects.Stop();
        blackScreen.SetTrigger(FadeIt);
    }
    
    private void OpenPauseMenu()
    {
        pauseMenuOn = true;
        pauseMenu.SetActive(true);
        PlayerController.Instance.DisableInputs();
        Cursor.visible = true;
        TargetCursor.instance.enabled = false;
    }
    public void ClosePauseMenu()
    {
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
}
