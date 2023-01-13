using System;
using DG.Tweening;
using UnityEngine;

public class AudioList : MonoBehaviour
{
    public enum Music
    {
        main,
        menu,
        combat,
        boss
    }
    
    public static AudioList Instance;

    [SerializeField] private AudioSource audioSource0;
    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private float fadeDuration;
    private bool _playOn1, _notFirstCall;
    private float _targetVolume;
    private AudioSource _currentSource;

    [Header("Global")]
    [SerializeField] private AudioClip mainTheme;
    [SerializeField] [Range(0, 1)] private float mainVolume;
    [SerializeField] private AudioClip menuTheme;
    [SerializeField] [Range(0, 1)] private float menuVolume;
    [SerializeField] private AudioClip combatTheme;
    [SerializeField] [Range(0, 1)] private float combatVolume;
    [SerializeField] private AudioClip bossBattleTheme;
    [SerializeField] [Range(0, 1)] private float bossVolume;

    [Header("Player")]
    public AudioClip basicAttack;
    public AudioClip stepSound;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    private void Start()
    {
        //Test
        StartMusic(Music.boss,true);
        _notFirstCall = true;
    }

    public void StartMusic(Music music, bool loop)
    {
        if(_notFirstCall) FadeOut();
        
        _currentSource = _playOn1? audioSource1 : audioSource0;
        _playOn1 = !_playOn1;
        
        switch (music)
        {
            case Music.main:
                _currentSource.clip = mainTheme;
                _targetVolume = mainVolume;
                break;
            case Music.menu:
                _currentSource.clip = menuTheme;
                _targetVolume = menuVolume;
                break;
            case Music.boss:
                _currentSource.clip = bossBattleTheme;
                _targetVolume = bossVolume;
                break;
            case Music.combat:
                _currentSource.clip = combatTheme;
                _targetVolume = combatVolume;
                break;
        }
        _currentSource.loop = loop;
        _currentSource.Play();
        
        FadeIn();
    }

    private void FadeIn()
    {
        _currentSource.DOFade(_targetVolume, fadeDuration);
    }

    private void FadeOut()
    {
        _currentSource.DOFade(0f, fadeDuration);
    }
}
