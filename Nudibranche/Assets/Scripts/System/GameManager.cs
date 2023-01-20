using Character;
using GenPro;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace System
{
    public class GameManager : MonoBehaviour
    {
        public float screenWidth = Screen.width;
        public float screenHeight = Screen.height;
        public static GameManager instance;
        public Camera mainCamera;
        public Slider bossGauge;
        [SerializeField] private Animator blackScreenAnim;
        public bool cheatDeath;

        [Header("Perles")]
        public int pearlAmount;

        [Header("State")]
        public bool inCombat;
        public bool inBossCutscene;

        public TMP_Text pearlAmountText;
        
        [Header("Ennemies Drop")]
        public int canonnierPearlDrop = 3;
        public int moulePearlDrop = 3;
        public int crevettePearlDrop = 3;
        public float canonnierLifeDrop = 0.5f;
        public float mouleLifeDrop = 0.1f;
        public float crevetteLifeDrop = 0.3f;

        //For Generation
        [HideInInspector] public int firstCharacterIndex = -1;

        //Position References
        public Vector3 hubPos;
        public Vector3 shopPos;
        public Vector3 bossPos;
        public Vector3 bossCinematicPos;
        void Awake()
        {
            if (instance != null && instance != this)
                Destroy(gameObject);
 
            instance = this;

            _startTime = Time.time;
        }
        void Start()
        {
            cheatDeath = false;
            blackScreenAnim.SetBool("Faded",false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            //pearlAmountText.text = 0 + "";
        }


        //Score

        [Header("Score")]
        [SerializeField] private int roomScore;
        
        private int _score;
        private float _startTime;
        private int _clearedRoomAmount;
        
        public void AddScore()
        {
            _score += roomScore;
        }

        public int FinalScore()
        {
            var endTime = Time.time - _startTime;
            _score = (int)((_clearedRoomAmount * 1500 + pearlAmount*20)-15*endTime);
            _score += 10000;
            return _score;
        }

        //Fonctions pour TP

        [HideInInspector] public RoomManager startRoom;
        [HideInInspector] public RoomManager shopRoom;
        [HideInInspector] public RoomManager bossRoom;
        [HideInInspector] public RoomManager currentRoom;

        public void ReloadStart()
        {
            currentRoom.Deactivate();
            startRoom.Activate();
            currentRoom = startRoom;
        }
        public void ReloadShop()
        {
            currentRoom.Deactivate();
            shopRoom.Activate();
            AudioList.Instance.StartMusic(AudioList.Music.character,true);
            currentRoom = shopRoom;
        }
        public void ReloadBoss()
        {
            Debug.Log(bossRoom);
            currentRoom.Deactivate();
            bossRoom.Activate();
            currentRoom = bossRoom;
        }
        
        //Cheat Codes
        public void CheatDeath()
        {
            cheatDeath = !cheatDeath;
        }

        public void MoreMoney()
        {
            pearlAmount += 100;
            pearlAmountText.text = "" + pearlAmount;
        }

        public void TpHub()
        {
            ReloadStart();
            PlayerController.Instance.transform.position = hubPos;
        }
        
        public void TpShop()
        {
            ReloadShop();
            PlayerController.Instance.transform.position = shopPos;
        }
        
        public void TpBoss()
        {
            ReloadBoss();
            PlayerController.Instance.transform.position = bossPos;
        }
        
        public void SkillSelectedScierano()
        {
            PlayerController.Instance.skillIndex = 0;
            UIManager.instance.UpdateSkillInfo();
        }
        public void SkillSelectedShellock()
        {
            PlayerController.Instance.skillIndex = 1;
            UIManager.instance.UpdateSkillInfo();
        }
        public void SkillSelectedSireine()
        {
            PlayerController.Instance.skillIndex = 2;
            UIManager.instance.UpdateSkillInfo();
        }
        
    }
}
