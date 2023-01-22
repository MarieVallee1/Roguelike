using Character;
using UnityEngine;
using UnityEngine.UI;

public class DashCooldownFeedback : MonoBehaviour
{
    private Image _barre;

    [SerializeField] private Image dashBarre;
    // Start is called before the first frame update
    void Start()
    {
        _barre = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance.nextTimeDash >= PlayerController.Instance.dashCooldown)
        {
            _barre.enabled = false;
            dashBarre.enabled = false;
        }
        else
        {
            _barre.enabled = true;
            dashBarre.enabled = true;
        }
        
        _barre.fillAmount = PlayerController.Instance.nextTimeDash/PlayerController.Instance.dashCooldown;
    }
}
