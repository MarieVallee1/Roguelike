using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI[] scoreTxt;

    public List<int> allScore;
    
    private void Awake()
    {
        if(instance != null && instance != this) Destroy(this);
        instance = this;

        for (int i = 0; i < 5; i++)
        {
            allScore.Add(PlayerPrefs.GetInt("score " + i));
        }
        
    }


    public void UpdateAllScore()
    {
        for (int i = 0; i < allScore.Count; i++)
        {
            if (GameManager.instance.FinalScore() > allScore[i])
            {
                allScore[i] = GameManager.instance.FinalScore();
                PlayerPrefs.SetInt("score " + i, allScore[i]);
                UpdateScoreUI();

                break;
            }
        }
    }
    private void UpdateScoreUI()
    {
        for (int i = 0; i < 5; i++)
        {
            scoreTxt[i].text = "" + PlayerPrefs.GetInt("score " + i);
        }
    }

    // public void ResetScore()
    // {
    //     if (PlayerController.Instance.characterInputs.Character.Interact.triggered)
    //     {
    //         for (int i = 0; i < 5; i++)
    //         {
    //             PlayerPrefs.SetInt("score " + i, 0);
    //             allScore[i] = PlayerPrefs.GetInt("score " + i);
    //         }
    //     }
    //     
    //     if (PlayerController.Instance.characterInputs.Character.Dash.triggered)
    //     {
    //         UpdateAllScore();
    //     }
    // }
}
