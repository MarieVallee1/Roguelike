using Character;
using DG.Tweening;
using UnityEngine;

namespace System
{
    public class MousePosition : MonoBehaviour
    {
        private Vector3 _screenPosition;
        private Vector3 _worldPosition;
        private Vector3 _characterPosition;
        private bool _gamepadOn;

        private void Update()
        {
            BookPosition();
        }

        private void BookPosition()
        {
            if (!_gamepadOn)
            {
                float angle = Mathf.Atan2( PlayerController.instance.aim.x ,PlayerController.instance.aim.y) * Mathf.Rad2Deg;
                transform.DORotate(new Vector3(0, 0, -angle), 0.5f);
            }
            //Same but with the gamepad
            if(_gamepadOn)
            {
                float angle = Mathf.Atan2(PlayerController.instance.aim.x, PlayerController.instance.aim.y) * Mathf.Rad2Deg;
                transform.DORotate(new Vector3(0, 0, -angle), 0.5f);
            }
        }
    }
}