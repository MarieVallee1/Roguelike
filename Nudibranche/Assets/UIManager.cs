using Character;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    [SerializeField] private TextMeshProUGUI skillInfo;
    //[SerializeField] private TextMeshProUGUI dialogueBox;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;
    }
    
    void Start()
    {
        UpdateSkillInfo();
    }
    
    public void UpdateSkillInfo()
    {
        if (PlayerController.instance.currentSkill == null) skillInfo.text = "Current Skill : Null";
        else
        {
            skillInfo.text = "Current Skill :" + PlayerController.instance.currentSkill.name;
        }
    }
}
