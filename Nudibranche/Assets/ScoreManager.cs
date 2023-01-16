using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private TextMeshProUGUI[] scoreTxt;

    public int[] allScore;
    
    private void Awake()
    {
        if(instance != null && instance != this) Destroy(this);
        instance = this;
    }
    

    public void UpdateAllScore()
    {
        for (int i = 0; i < allScore.Length; i++)
        {
            if (GameManager.instance.FinalScore() > allScore[i])
            {
                allScore[i] = GameManager.instance.FinalScore();
                PlayerPrefs.SetInt("score " + i, allScore[i]);
                
                //Updates UI
                scoreTxt[i].text = "" + PlayerPrefs.GetInt("score " + i);
                
                break;
            }
        }
    }
}
