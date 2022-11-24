using DG.Tweening;
using UnityEngine;

namespace Character
{
    public class BookPosition : MonoBehaviour
    {
        [SerializeField] private Sprite[] bookSprites;
        [SerializeField] private SpriteRenderer ren;
        
        private Vector3 _screenPosition;
        private Vector3 _worldPosition;
        private Vector3 _characterPosition;

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
            float degrees = radians * Mathf.Rad2Deg;


            if(degrees < 35 && degrees > -35)
            {
                //Debug.Log("up");
                ren.sprite = bookSprites[0];
                ren.flipX = true;
            }
            
            if(degrees > 35 && degrees < 55)
            {
                //Debug.Log("up right");
                ren.sprite = bookSprites[1];
                ren.flipX = true;
            } 
            
            if(degrees > 55 && degrees < 125)
            {
                //Debug.Log("right");
                ren.sprite = bookSprites[2];
                ren.flipX = true;
            } 
            
            if(degrees > 125 && degrees < 145)
            {
                //Debug.Log("down right");
                ren.sprite = bookSprites[3];
                ren.flipX = true;
            } 
            
            if(degrees > 145 || degrees < -145)
            {
                //Debug.Log("down");
                ren.sprite = bookSprites[4];
                ren.flipX = true;
            }

            if (degrees > -145 && degrees < -125)
            {
                //Debug.Log("down left");
                ren.sprite = bookSprites[5];
                ren.flipX = false;
            }

            if (degrees > -125 && degrees < -55)
            {
                //Debug.Log("left");
                ren.sprite = bookSprites[6];
                ren.flipX = false;
            }

            if (degrees > -55 && degrees < -35)
            {
                //Debug.Log("up left");
                ren.sprite = bookSprites[7];
                ren.flipX = false;
            }
        }
    }
}