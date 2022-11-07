using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private int roundedYPos;
    void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }
    void Update()
    {
        roundedYPos = (int)Mathf.Round(transform.position.y * 10);
        //spriteRenderer.sortingOrder = - roundedYPos;
    }
}
