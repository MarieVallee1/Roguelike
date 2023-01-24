using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Video;

public class IntroductionCinematic : MonoBehaviour
{
    public Image[] images;
    [TextArea(3, 10)]
    public string[] narratorSentences;
    public TextMeshProUGUI narratorTxt;
    public CanvasGroup imagesGroup;
    public VideoPlayer videoPlayer;
    public Image blackScreen;
    private int step;
    public Image eToInteract;
    
    //Fix Intro
    private Tween _blackScreenTween;

    private void Start()
    {
        if (!AudioList.Instance.cinematicPlayed)
        {
            AudioList.Instance.cinematicPlayed = true;
            blackScreen.DOFade(0, 0.5f);
            PlayerController.Instance.DisableInputs();
            StartCoroutine(startCinematic());
            AudioList.Instance.PlayOneShot(AudioList.Instance.pageFlip,AudioList.Instance.pageFlipVolume);
        }
        else
        {
            _blackScreenTween = blackScreen.DOFade(0, 0.5f);
            imagesGroup.alpha = 0;
        }
    }
    
    void DisplayImage(int imageIndex)
    {
        images[imageIndex].DOFade(1, 1);
    }
    void DisplayText (int sentenceIndex)
    {
        narratorTxt.text = narratorSentences[sentenceIndex];
        narratorTxt.DOFade(1, 1);
    }

    private void Update()
    {
        if(PlayerController.Instance.characterInputs.UI.Interact.triggered)
        {
            step += 1;

            if (step == 1)
            {
                _blackScreenTween.Kill();
                var value = blackScreen.color;
                value = new Color(value.r,value.g,value.b,0f);
                blackScreen.color = value;
                images[0].DOFade(1, 0.1f);
                DisplayImage(1);
            }

            if (step == 2)
            {
                StartCoroutine(NextSlide(0, 1, 2, 1));
            }

            if (step == 3)
            {
                StartCoroutine(StepThree(2));
            }

            if (step == 4)
            {
                StartCoroutine(NextSlide(2, 3, 4, 3));
            }

            if (step == 5)
            {
                StartCoroutine(StepThree(4));
            }
            
            if (step <= 6)
            {
                StartCoroutine(endCinematic());
                PlayerController.Instance.EnableInputs();
            }
        }
    }

    IEnumerator startCinematic()
    {
        yield return new WaitForSeconds(0.5f);
        DisplayImage(step);
        DisplayText(step);
    }
    IEnumerator NextSlide(int firstPicture, int secondPicture, int pictureIndex, int textIndex)
    {
        images[firstPicture].DOFade(0, 1);
        images[secondPicture].DOFade(0, 1);
        narratorTxt.DOFade(0, 1);
        AudioList.Instance.PlayOneShot(AudioList.Instance.pageFlip,AudioList.Instance.pageFlipVolume);
        yield return new WaitForSeconds(1);
        DisplayImage(pictureIndex);
        DisplayText(textIndex);

        if (step == 4)
        {
            StartCoroutine(BookFall());
        }
    }

    IEnumerator StepThree(int textIndex)
    {
        narratorTxt.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        if (step == 3)
        {
            DisplayImage(3);
        }
        DisplayText(textIndex);
    }

    IEnumerator BookFall()
    {
        yield return new WaitForSeconds(1);
        images[4].DOFade(0,1);
        DisplayImage(5);
        yield return new WaitForSeconds(1);
        images[5].DOFade(0,1);
        DisplayImage(6);
    }

    IEnumerator endCinematic()
    {
        videoPlayer.Play();
        yield return new WaitForSeconds(0.2f);
        videoPlayer.gameObject.GetComponent<RawImage>().enabled = true;
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        blackScreen.DOFade(1, 0);
        eToInteract.DOFade(0, 1);
        AudioList.Instance.PlayOneShot(AudioList.Instance.pageFlip,AudioList.Instance.pageFlipVolume);
        yield return new WaitForSeconds(1);
        videoPlayer.gameObject.SetActive(false);
        imagesGroup.gameObject.SetActive(false);
        narratorTxt.DOFade(0, 0);
        blackScreen.DOFade(0, 1);
        videoPlayer.gameObject.GetComponent<RawImage>().enabled = false;
    }
}
