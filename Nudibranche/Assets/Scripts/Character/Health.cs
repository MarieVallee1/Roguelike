using System;
using Character;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static Health instance;
    
    public int numberOfHearts;
    public int health;
    
    [SerializeField] private Image[] hearts;
    [SerializeField] private Animation anim;
    
    public Sprite fullHeart;
    public Sprite emptyHeart;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        instance = this;
    }
    private void Update()
    {
        if (health > hearts.Length) health = hearts.Length;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health) hearts[i].sprite = fullHeart;
            else hearts[i].sprite = emptyHeart;

            if (i < numberOfHearts) hearts[i].enabled = true;
            else hearts[i].enabled = false;
        }
    }
    
    public void SetHealth(int playerHealth)
    {
        anim.Play();
        health = playerHealth;
    }
}
