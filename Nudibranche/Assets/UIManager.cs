using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    [SerializeField] private TextMeshProUGUI skillInfo;
    //[SerializeField] private TextMeshProUGUI dialogueBox;
    [SerializeField] private ParticleSystem slashEffects;
    [SerializeField] private Animator blackScreen;
    private static readonly int FadeIt = Animator.StringToHash("FadeIt");


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
    }
    
    public void UpdateSkillInfo()
    {
        skillInfo.text = "Current Skill :" + PlayerController.instance.currentSkill;
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
    
}
