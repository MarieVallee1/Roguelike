using UnityEngine;

public class MousePosition : MonoBehaviour
{
    private Vector3 _screenPosition;
    private Vector3 _worldPosition;
    [SerializeField] private CharacterController cc;


    private void Start()
    {
        
    }

    private void Update()
    {
        _screenPosition = Input.mousePosition;
        _screenPosition.z = Camera.main.nearClipPlane + 1;
        _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);
        transform.position = _worldPosition;
    }

    private void BookPosition()
    {
        
    }
}