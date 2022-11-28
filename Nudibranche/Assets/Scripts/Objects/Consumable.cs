using Character;
using UnityEngine;

namespace Objects
{
    public class Consumable : Reward
    {
        private bool _inZone;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != 6) return;
            _inZone = true;
        }  
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer != 6) return;
            _inZone = false;
        }
    
        private void Update()
        {
            if (_inZone)
            {
                InteractionZone();
            }
        }
    
        private void InteractionZone()
        {
            if (PlayerController.Instance.characterInputs.Character.Interact.triggered)
            {
                OnPickUp();
            }
        }

        private void OnPickUp()
        {
            ItemManager.Instance.CheckConsumable(consumableIndex);
            UIManager.instance.UpdateObjectInfo();
            Destroy(gameObject);
        }
    }
}
