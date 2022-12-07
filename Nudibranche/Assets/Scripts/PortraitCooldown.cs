using Character;
using UnityEngine;
using UnityEngine.UI;

public class PortraitCooldown : MonoBehaviour
{
   private Image _visualCooldown;
   private PlayerController _pc;

   private void Start()
   {
      _pc = PlayerController.Instance;
      _visualCooldown = GetComponent<Image>();
   }

   private void Update()
   {
      if (_pc.skillCountdown < _pc.skillCooldown)
      {
         _visualCooldown.enabled = true;
         SetCooldown();
      }
      else _visualCooldown.enabled = false;
   }

   private void SetCooldown()
   {
      _visualCooldown.fillAmount = 1 - _pc.skillCountdown / _pc.skillCooldown ;
   }
}
