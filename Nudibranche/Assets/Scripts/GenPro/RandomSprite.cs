using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GenPro
{
    public class RandomSprite : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;

        private void Awake()
        {
            GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }
}
