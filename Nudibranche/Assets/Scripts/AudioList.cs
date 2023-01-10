using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioList : MonoBehaviour
{
    public static AudioList Instance;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sons;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }
    
    public void PlaySound(int index)
    {
        audioSource.PlayOneShot(sons[index]);
    }
}
