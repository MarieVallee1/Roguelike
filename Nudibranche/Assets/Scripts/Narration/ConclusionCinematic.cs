using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class ConclusionCinematic : MonoBehaviour
{
    public static ConclusionCinematic instance;
    
    public Image paperBackground;
    public Image conclusionImage;
    [TextArea(3, 10)]
    public string[] narratorSentences;
    public TextMeshProUGUI narratorTxt;
    public VideoPlayer videoPlayer;
    public Image blackscreen;
    
    //crédits
    //score

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
 
        instance = this;
    }

    public void StartConclusionCinematic()
    {
        StartCoroutine(StartCinematic());
    }

    IEnumerator StartCinematic()
    {
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();
        yield return new WaitForSeconds(0.2f);
        videoPlayer.gameObject.GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        blackscreen.DOFade(1, 0);
        paperBackground.DOFade(1, 0);
        yield return new WaitForSeconds(1);
        videoPlayer.gameObject.SetActive(false);
        blackscreen.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        conclusionImage.DOFade(1, 1);
        narratorTxt.text = narratorSentences[0];
        narratorTxt.DOFade(1, 1);
        yield return new WaitForSeconds(3);
        StartCoroutine(NextText(1));
        yield return new WaitForSeconds(4);
        StartCoroutine(NextText(2));
        yield return new WaitForSeconds(3);
        Debug.Log("crédits");
        SceneManager.LoadScene("Scene_MainMenu");
    }

    IEnumerator NextText(int textIndex)
    {
        narratorTxt.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        narratorTxt.text = narratorSentences[textIndex];
        narratorTxt.DOFade(1, 1);
    }
}
