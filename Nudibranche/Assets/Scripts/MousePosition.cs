using System;
using UnityEngine;
using CharacterController = Character.CharacterController;

public class MousePosition : MonoBehaviour
{
    private Vector3 _screenPosition;
    private Vector3 _worldPosition;
    
    [SerializeField] private Camera cam;
    private CharacterController _cc;

    private void Start()
    {
        _cc = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        BookPosition();
    }

    private void BookPosition()
    {
        //Handle the position of the book if the player is shooting with the mouse
        
        _screenPosition = Input.mousePosition;
        _screenPosition.z = cam.nearClipPlane + 1;
        _worldPosition = cam.ScreenToWorldPoint(_screenPosition);
        
        if (_cc.isShootingMouse)
        {
            float angle = Mathf.Atan2(_worldPosition.x, _worldPosition.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
        //Same but with the gamepad
        else if(_cc.isShootingGamepad)
        {
            float angle = Mathf.Atan2(_cc.aim.x, _cc.aim.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
    }
}