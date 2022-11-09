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
            HandleSpriteRotation();
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
            if(transform.rotation.x < 10 && transform.rotation.x > -10)
            {
                print("is up ");
            }
            
            if(transform.rotation.x > 10 && transform.rotation.x < 80)
            {
                print("is up left");
            } 
            
            if(transform.rotation.x > 80 && transform.rotation.x < 100)
            {
                print("is left");
            } 
            
            if(transform.rotation.x > 100 && transform.rotation.x < 170)
            {
                print("is down left");
            } 
            
            if(transform.rotation.x > 170 && transform.rotation.x < -170)
            {
                print("is down");
            }

            if (transform.rotation.x < -170 && transform.rotation.x < -80)
            {
                print("is down right");
            }

            if (transform.rotation.x > -80 && transform.rotation.x < -70)
            {
                print("is right");
            }

            if (transform.rotation.x > -70 && transform.rotation.x > -10)
            {
                print("is up right");
            }
        }
    }
}