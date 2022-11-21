using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    public Image blackScreen;
    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        
        //set the alpha to 1
        blackScreen.color = new Color(0,0,0,1);
    }

    private void OnEnable()
    {
        _inputActions.Character.Disable();
        _inputActions.UI.Enable();
    }

    void Start()
    {
        blackScreen.DOFade(0, 3f).onComplete = DisableBlackScreen;
    }
    private void DisableBlackScreen()
    {
        blackScreen.enabled = false;
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Scene_Terri");
    }

    public void OptionsButton()
    {
        
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
