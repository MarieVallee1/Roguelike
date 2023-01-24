using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI[] scoreTxt;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI finalScore;
    public List<int> allScore;
    
    private void Awake()
    {
        if(instance != null && instance != this) Destroy(this);
        instance = this;

        for (int i = 0; i < 5; i++)
        {
            allScore.Add(PlayerPrefs.GetInt("score " + i,0));
        }
        
    }


    public void UpdateAllScore()
    {
        UpdateScoreUI();
        for (int i = 0; i < allScore.Count; i++)
        {
            if (GameManager.instance.FinalScore() > allScore[i])
            {
                allScore[i] = GameManager.instance.FinalScore();
                PlayerPrefs.SetInt("score " + i, allScore[i]);

                break;
            }
        }
    }

    private void UpdateScoreUI()
    {
        finalScore.text = "" + GameManager.instance.FinalScore();
      
        TimeSpan timeSpan = TimeSpan.FromSeconds(GameManager.instance.endTime);
        timer.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }
    
    public void ScoreboardUI()
    {
        for (int i = 0; i < 5; i++)
        {
            scoreTxt[i].text = "" + PlayerPrefs.GetInt("score " + i);
        }
    }

}
