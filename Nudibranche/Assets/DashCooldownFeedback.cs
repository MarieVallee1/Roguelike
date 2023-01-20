using Character;
using UnityEngine;
using UnityEngine.UI;

public class DashCooldownFeedback : MonoBehaviour
{
    private Image _barre;
    // Start is called before the first frame update
    void Start()
    {
        _barre = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.time / PlayerController.Instance.nextTimeDash);
        Debug.Log(Time.time);
        Debug.Log( PlayerController.Instance.nextTimeDash);
        _barre.fillAmount = Time.time / PlayerController.Instance.nextTimeDash;
    }
}
