using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Character
{
    public class Bait : MonoBehaviour
    {
        private float _countdown;
        private SpriteRenderer _ren;
        [SerializeField] private float baitDuration;

        private void Start()
        {
            _ren = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _countdown = 0;

            //_ren.DOFade(1, 1);
        }

        private void Update()
        {
            if (_countdown < baitDuration) _countdown += Time.deltaTime;
            else
            {
                //_ren.DOFade(0, 1f);
                StartCoroutine(BaitFade());
            }
        }


        private IEnumerator BaitFade()
        {
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
        }
    }

    #region Dissolve material

    // private void OnEnable()
    // {
    //     _countdown = 0;
    //     _shaderDissolveValue = 1;
    // }
    //
    // private void Update()
    // {
    //     if (_countdown < baitDuration)
    //     {
    //         _countdown += Time.deltaTime;
    //     }
    //     else
    //     {
    //         for (int i = 0; i < sprites.Length; i++)
    //         {
    //             sprites[i].SetFloat("_Dissolve", _shaderDissolveValue);
    //         }
    //             
    //         StartCoroutine(BaitFade());
    //     }
    // }
    //
    // private IEnumerator BaitFade()
    // {
    //     DOTween.To(()=> _shaderDissolveValue, x=> _shaderDissolveValue = x, -1, 1);
    //     yield return new WaitForSeconds(2.5f);
    //     gameObject.SetActive(false);
    // }

    #endregion
    
}
