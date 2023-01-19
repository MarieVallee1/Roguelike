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
        boss,
        character,
        ending
    }
    
    public static AudioList Instance;

    [SerializeField] private AudioSource audioSource0;
    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSourceOneShot;
    [SerializeField] private float fadeDuration;
    private bool _playOn1, _notFirstCall;
    [HideInInspector] public bool inCombat;
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
    [SerializeField] private AudioClip characterTheme;
    [SerializeField] [Range(0, 1)] private float characterVolume;
    [SerializeField] private AudioClip endingTheme;
    [SerializeField] [Range(0, 1)] private float endingVolume;

    [Header("UI")]
    public AudioClip uiClick;

    [Header("Player")]
    public AudioClip basicAttack;
    public AudioClip playerDash;

    [Header("Enemy")]
    public AudioClip enemyHit;
    public AudioClip enemyDeath;
    public AudioClip urchinLaunch;
    public AudioClip urchinLanding;
    public AudioClip crevetteShot;
    public AudioClip mouleSword;

    [Header("Non-spatialize")]
    public AudioClip playerHit;
    [Range(0, 1)] public float playerHitVolume;
    public AudioClip buyInShop;
    [Range(0, 1)] public float buyInShopVolume;
    public AudioClip pageFlip;
    [Range(0, 1)] public float pageFlipVolume;
    public AudioClip playerHeal;
    [Range(0, 1)] public float playerHealVolume;
    public AudioClip pearlCollect;
    [Range(0, 1)] public float pearlCollectVolume;
    public AudioClip scierano;
    [Range(0, 1)] public float scieranoVolume;
    public AudioClip scieranoEnd;
    [Range(0, 1)] public float scieranoEndVolume;
    public AudioClip sireineDeCoeur;
    [Range(0, 1)] public float sireineDeCoeurVolume;
    public AudioClip shellock;
    [Range(0, 1)] public float shellockVolume;
    
    // Bool pour éviter de rejouer la cinématique
    [HideInInspector] public bool cinematicPlayed;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        //Test
        StartMusic(Music.menu,true);
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
                inCombat = true;
                break;
            case Music.character:
                _currentSource.clip = characterTheme;
                _targetVolume = characterVolume;
                break;
            case Music.ending:
                _currentSource.clip = endingTheme;
                _targetVolume = endingVolume;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(music), music, null);
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

    public void PlayOneShot(AudioClip clip, float volumeScale)
    {
        audioSourceOneShot.PlayOneShot(clip,volumeScale);
    }
}
