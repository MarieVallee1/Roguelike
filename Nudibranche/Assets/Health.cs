using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static Health instance;

    public int health;
    public int numberOfHearts;
    
    [SerializeField] private Image[] hearts;
    
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
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health) hearts[i].sprite = fullHeart;

            if (i < numberOfHearts) hearts[i].enabled = true;
            else hearts[i].enabled = false;
        }
    }
}
