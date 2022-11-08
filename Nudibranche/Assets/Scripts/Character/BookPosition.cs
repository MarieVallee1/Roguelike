using System;
using DG.Tweening;
using UnityEngine;

namespace Character
{
    public class BookPosition : MonoBehaviour
    {
        [SerializeField] private Sprite[] bookSprites;
        private SpriteRenderer _ren;
        
        private Vector3 _screenPosition;
        private Vector3 _worldPosition;
        private Vector3 _characterPosition;

        private void Awake()
        {
            _ren = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            SetRotation();
        }

        private void SetRotation()
        {
            if (!PlayerController.instance.gamepadOn)
            {
                float angle = Mathf.Atan2( PlayerController.instance.aim.x ,PlayerController.instance.aim.y) * Mathf.Rad2Deg;
                transform.DORotate(new Vector3(0, 0, -angle), 0.5f);
            }
            //Same but with the gamepad
            if(PlayerController.instance.gamepadOn)
            {
                float angle = Mathf.Atan2(PlayerController.instance.aim.x, PlayerController.instance.aim.y) * Mathf.Rad2Deg;
                transform.DORotate(new Vector3(0, 0, -angle), 0.5f);
            }
        }

        private void HandleSpriteRotation()
        {
            
        }
    }
}