using System;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager instance;

    [SerializeField] private GameObject[] videoDisplay;
    [SerializeField] private VideoPlayer[] videoPlayer;
    [SerializeField] private VideoClip[] videoClip;


    private void Awake()
    {
        if(instance != null && instance != this) Destroy(this);
        instance = this;
    }
    


    public void PlayVideo(int videoDisplayIndex,int videoPlayerIndex, int videoClipIndex)
    {
        videoPlayer[videoPlayerIndex].clip = videoClip[videoClipIndex];
        videoDisplay[videoDisplayIndex].SetActive(true);
        videoPlayer[videoPlayerIndex].Play();
        
        VideoDisplayDeactivation(videoDisplayIndex,videoPlayerIndex);
    }

    private void VideoDisplayDeactivation(int videoDisplayIndex,int videoPlayerIndex)
    {
        if (videoPlayer[videoPlayerIndex].isPlaying)
        {
            Debug.Log("It works 1");
        }
    }

}
