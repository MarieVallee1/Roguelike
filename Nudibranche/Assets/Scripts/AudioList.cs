using System;
using UnityEngine;

public class AudioList : MonoBehaviour
{
    public enum Music
    {
        main,
        menu,
        boss
    }
    
    public static AudioList Instance;

    [SerializeField] private AudioSource audioSource;

    [Header("Global")]
    [SerializeField] private AudioClip mainTheme;
    [SerializeField] [Range(0, 1)] private float mainVolume;
    [SerializeField] private AudioClip menuTheme;
    [SerializeField] [Range(0, 1)] private float menuVolume;
    [SerializeField] private AudioClip bossBattleTheme;
    [SerializeField] [Range(0, 1)] private float bossVolume;

    [Header("Player")]
    public AudioClip basicAttack;
    public AudioClip stepSound;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
        
        //Test
        StartMusic(Music.boss,true);
    }

    public void StartMusic(Music music, bool loop)
    {
        switch (music)
        {
            case Music.main:
                audioSource.clip = mainTheme;
                audioSource.volume = mainVolume;
                break;
            case Music.menu:
                audioSource.clip = menuTheme;
                audioSource.volume = menuVolume;
                break;
            case Music.boss:
                audioSource.clip = bossBattleTheme;
                audioSource.volume = bossVolume;
                break;
            default:
                break;
        }
        audioSource.loop = loop;
        audioSource.Play();
    }
}
