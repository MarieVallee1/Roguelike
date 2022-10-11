using System;
using Character;
using TreeEditor;
using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    [SerializeField] private Camera cam;
    public static TargetCursor instance;
    

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            instance = this;
        }
    }

    private void Update()
    {
        CursorPosition();
        LookAheadPosition();
    }

    private void LookAheadPosition()
    {

    }

    private void CursorPosition()
    {
        transform.position = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x,cam.ScreenToWorldPoint(Input.mousePosition).y,-1);
    }
}
