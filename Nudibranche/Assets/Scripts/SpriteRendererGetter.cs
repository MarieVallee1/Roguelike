using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class SpriteRendererGetter : MonoBehaviour
{
    private PlayerController _pc;
    
    void Start()
    {
        _pc = PlayerController.Instance;
        
        _pc.visuals.Add(GetComponent<SpriteRenderer>());
    }
    
}
