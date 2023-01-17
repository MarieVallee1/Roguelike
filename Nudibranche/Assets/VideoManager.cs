using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager instance;
    
    [SerializeField] private VideoPlayer[] videoPlayer;
    [SerializeField] private VideoClip[] videoClip;


    private void Awake()
    {
        if(instance != null && instance != this) Destroy(this);
        instance = this;
    }

    public void PlayVideo(int videoPlayerIndex, int videoClipIndex)
    {
        videoPlayer[videoPlayerIndex].clip = videoClip[videoClipIndex];
        videoPlayer[videoPlayerIndex].Play();
    }

}
