using UnityEngine;

public class AudioList : MonoBehaviour
{
    public static AudioList Instance;

    [SerializeField] private AudioSource audioSource;

    [Header("Player")]
    public AudioClip stepSound;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    public void StartMusic()
    {
        audioSource.Play();
    }
}
