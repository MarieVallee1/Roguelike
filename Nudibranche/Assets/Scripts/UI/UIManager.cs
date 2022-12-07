using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using DG.Tweening;
using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private List<GameObject> portraits;

    [SerializeField] private TextMeshProUGUI objectInfo;
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
    
    public void BlackScreenFade()
    {
        blackScreen.SetTrigger(FadeIt);
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
