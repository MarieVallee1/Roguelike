using System;
using DG.Tweening;
using UnityEngine;

namespace Character
{
    public class BookPosition : MonoBehaviour
    {
        public static BookPosition Instance;
        
        [SerializeField] private Sprite[] bookSprites;
        [SerializeField] private SpriteRenderer ren;
        
        private Vector3 _screenPosition;
        private Vector3 _worldPosition;
        private Vector3 _characterPosition;

        public float directionAngle;

        private void Awake()
        {
            if(Instance != null && Instance != this) Destroy(this);
            Instance = this;
        }

        private void Update()
        {
            SetRotation();
            HandleSpriteRotation();
        }

        private void SetRotation()
        {
            if (!PlayerController.Instance.gamepadOn)
            {
                float angle = Mathf.Atan2( PlayerController.Instance.aim.x ,PlayerController.Instance.aim.y) * Mathf.Rad2Deg;
                transform.DORotate(new Vector3(0, 0, -angle), 0.5f);
            }
            //Same but with the gamepad
            if(PlayerController.Instance.gamepadOn)
            {
                float angle = Mathf.Atan2(PlayerController.Instance.aim.x, PlayerController.Instance.aim.y) * Mathf.Rad2Deg;
                transform.DORotate(new Vector3(0, 0, -angle), 0.5f);
            }
        }

        private void HandleSpriteRotation()
        {
            // get the raw angle, in radians
            float radians = Mathf.Atan2 (PlayerController.Instance.aim.x, PlayerController.Instance.aim.y);
 
            // up to degrees
            directionAngle = radians * Mathf.Rad2Deg;


            if(directionAngle < 35 && directionAngle > -35)
            {
                //Debug.Log("up");
                ren.sprite = bookSprites[0];
                ren.flipX = true;
            }
            
            if(directionAngle > 35 && directionAngle < 55)
            {
                //Debug.Log("up right");
                ren.sprite = bookSprites[1];
                ren.flipX = true;
            } 
            
            if(directionAngle > 55 && directionAngle < 125)
            {
                //Debug.Log("right");
                ren.sprite = bookSprites[2];
                ren.flipX = true;
            } 
            
            if(directionAngle > 125 && directionAngle < 145)
            {
                //Debug.Log("down right");
                ren.sprite = bookSprites[3];
                ren.flipX = true;
            } 
            
            if(directionAngle > 145 || directionAngle < -145)
            {
                //Debug.Log("down");
                ren.sprite = bookSprites[4];
                ren.flipX = true;
            }

            if (directionAngle > -145 && directionAngle < -125)
            {
                //Debug.Log("down left");
                ren.sprite = bookSprites[5];
                ren.flipX = false;
            }

            if (directionAngle > -125 && directionAngle < -55)
            {
                //Debug.Log("left");
                ren.sprite = bookSprites[6];
                ren.flipX = false;
            }

            if (directionAngle > -55 && directionAngle < -35)
            {
                //Debug.Log("up left");
                ren.sprite = bookSprites[7];
                ren.flipX = false;
            }
        }
    }
}