using System;
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
        transform.position = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x,cam.ScreenToWorldPoint(Input.mousePosition).y,-1);
    }
}
