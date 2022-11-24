using Character;
using UnityEngine;

namespace System
{
    public class TargetCursor : MonoBehaviour
    {
        private Camera cam;
        public static TargetCursor instance;

        private void Awake()
        {
            if (instance != null && instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                instance = this; 
            } 
            
            cam = Camera.main;
        }
        private void Update()
        {
            if(!PlayerController.Instance.gamepadOn) CursorPosition();
        }
        

        private void CursorPosition()
        {
            transform.position = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x,cam.ScreenToWorldPoint(Input.mousePosition).y,-1);
        }
    }
}
