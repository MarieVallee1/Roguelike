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
    [SerializeField] private Image victoryScreen;
    [SerializeField] private Sprite victorySprite;
    public GameObject credits;
    public float creditsSpeed;
    public float creditsYPosEnd;
    private CanvasGroup loreCanvas;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
 
        instance = this;

        loreCanvas = GetComponent<CanvasGroup>();
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
        videoPlayer.gameObject.GetComponent<RawImage>().enabled = true;
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        blackscreen.DOFade(1, 0);
        paperBackground.DOFade(1, 0);
        AudioList.Instance.PlayOneShot(AudioList.Instance.pageFlip,AudioList.Instance.pageFlipVolume);
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

        credits.transform.DOLocalMoveY(creditsYPosEnd, creditsSpeed);
        yield return new WaitForSeconds(creditsSpeed + 1);
        
        //Score Screen
        ChangeDeathToVictoryScreen();
        ScoreManager.instance.UpdateAllScore();
        UIManager.instance.OpenDeathScreen();
        UIManager.instance.BlackScreenFadeOut();
        conclusionImage.DOFade(0, 0.5f);
        narratorTxt.DOFade(0, 0.5f);
        paperBackground.DOFade(0, 0.5f);
        loreCanvas.DOFade(0, 0.5f);
    }

    private void ChangeDeathToVictoryScreen()
    {
        victoryScreen.sprite = victorySprite;
    }

    IEnumerator NextText(int textIndex)
    {
        narratorTxt.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        narratorTxt.text = narratorSentences[textIndex];
        narratorTxt.DOFade(1, 1);
    }
}
