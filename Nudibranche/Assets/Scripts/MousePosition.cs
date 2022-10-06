using DG.Tweening;
using UnityEngine;
using CharacterController = Character.CharacterController;

public class MousePosition : MonoBehaviour
{
    private Vector3 _screenPosition;
    private Vector3 _worldPosition;
    private Vector3 _characterPosition;
    private bool _gamepadOn;
    
    private CharacterController _cc;

    private void Start()
    {
        _cc = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        BookPosition();
        LastControlUsed();
    }

    private void BookPosition()
    {
        if (!_gamepadOn)
        {
            float angle = Mathf.Atan2( _cc.aim.x,_cc.aim.y) * Mathf.Rad2Deg;
            transform.DORotate(new Vector3(0, 0, -angle), 0.5f);
        }
        //Same but with the gamepad
        if(_gamepadOn)
        {
            float angle = Mathf.Atan2(_cc.aim.x, _cc.aim.y) * Mathf.Rad2Deg;
            transform.DORotate(new Vector3(0, 0, -angle), 0.5f);
        }
    }

    private void LastControlUsed()
    {
        if (_cc.isShootingGamepad) _gamepadOn = true;
        if (_cc.isShootingMouse) _gamepadOn = false;
    }
}