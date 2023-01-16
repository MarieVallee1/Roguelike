using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


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
    public void UpdateScoreUI()
    {
        for (int i = 0; i < 5; i++)
        {
            scoreTxt[i].text = "" + PlayerPrefs.GetInt("score " + i);
        }
    }
    
}
