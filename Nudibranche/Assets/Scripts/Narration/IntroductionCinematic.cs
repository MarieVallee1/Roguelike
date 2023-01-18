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

    private int step;

   /* If(PlayerController.instance.characterinput.character.interact.triggered && PlayerController.instance.characterinput.UI.interact.triggered)
    {
        
    } */

    private void Start()
    {
        DisplayImage(step);
        DisplayText(step);
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
        if(PlayerController.Instance.characterInputs.Character.Interact.triggered)
        {
            step += 1;

            if (step == 1)
            {
                DisplayImage(1);
            }

            if (step == 2)
            {
                StartCoroutine(NextSlide(0, 1, 2, 1));
            }

            if (step == 3)
            {
                StartCoroutine(StepThree());
            }

            if (step == 4)
            {
                StartCoroutine(NextSlide(2, 3, 4, 3));
            }
        }
    }
    IEnumerator NextSlide(int firstPicture, int secondPicture, int pictureIndex, int textIndex)
    {
        images[firstPicture].DOFade(0, 1);
        images[secondPicture].DOFade(0, 1);
        narratorTxt.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        DisplayImage(pictureIndex);
        DisplayText(textIndex);

        if (step == 4)
        {
            StartCoroutine(BookFall());
        }
    }

    IEnumerator StepThree()
    {
        narratorTxt.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        DisplayImage(3);
        DisplayText(2);
    }

    IEnumerator BookFall()
    {
        yield return new WaitForSeconds(1);
        images[4].DOFade(0,1);
        DisplayImage(5);
        yield return new WaitForSeconds(1);
        images[5].DOFade(0,1);
        DisplayImage(6);
        yield return new WaitForSeconds(2);
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();
    }
}
