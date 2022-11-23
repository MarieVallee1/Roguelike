using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    private EventSystem _event;
    [SerializeField] private RectTransform cursor;
    private string _selectedButtons;
    public Image blackScreen;
    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _event = EventSystem.current;
        
        //set the alpha to 1
        blackScreen.color = new Color(0, 0, 0, 1);
    }

    private void OnEnable()
    {
        _inputActions.Character.Disable();
        _inputActions.UI.Enable();
    }

    void Start()
    {
        blackScreen.DOFade(0, 3f).onComplete = DisableBlackScreen;
    }

    private void Update()
    {
        HandleSelectedButtons();
    }

    private void DisableBlackScreen()
    {
        blackScreen.enabled = false;
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Scene_Terri");
    }

    public void OptionsButton()
    {
        
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void HandleSelectedButtons()
    {
        switch (_event.name)
        {
            case "PlayButton":
            {
                cursor.DOAnchorPosY(0, 0.5f);
            }
                break;
            
            case "OptionsButton":
            {
                cursor.DOAnchorPosY(-80, 0.5f);
            }
                break;
            
            case "QuitButton":
            {
                cursor.DOAnchorPosY(-160, 0.5f);
            }
                break;
        }
    }
}
