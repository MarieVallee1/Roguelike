using UnityEngine;
using TMPro;

namespace System
{
    public class GameManager : MonoBehaviour
    {
        public float screenWidth = Screen.width;
        public float screenHeight = Screen.height;
        public static GameManager instance;
        public Camera mainCamera;
        
        
        [Header("Perles")]
        public int pearlAmount;

        public TMP_Text pearlAmountText;
        
        [Header("Ennemies Drop")]
        public int canonnierPearlDrop = 3;
        public int moulePearlDrop = 3;
        public int crevettePearlDrop = 3;
        public float canonnierLifeDrop = 0.5f;
        public float mouleLifeDrop = 0.1f;
        public float crevetteLifeDrop = 0.3f;


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
            //pearlAmountText.text = 0 + "";
        }
    }
}
