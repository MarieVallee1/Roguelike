using System;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    private Vector3 _screenPosition;
    private Vector3 _worldPosition;

    private void Update()
    {
        _screenPosition = Input.mousePosition;
        _screenPosition.z = Camera.main.nearClipPlane + 1;

        _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);

        transform.position = _worldPosition;
    }
}
