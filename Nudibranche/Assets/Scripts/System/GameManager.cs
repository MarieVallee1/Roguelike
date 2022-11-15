using UnityEngine;

namespace System
{
    public class GameManager : MonoBehaviour
    {
        public float screenWidth = Screen.width;
        public float screenHeight = Screen.height;
        public static GameManager instance;


        void Awake()
        {
            if (instance != null && instance != this)
                Destroy(gameObject);
 
            instance = this;
        }
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
