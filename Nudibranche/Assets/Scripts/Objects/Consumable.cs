using Character;
using UnityEngine;

namespace Objects
{
    public class Consumable : Reward
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private bool _inZone;

        private void Awake()
        {
            spriteRenderer.sprite = stats.objectImage;
        }
        
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
                OnAcquire();
            }
        }

        public override void OnAcquire()
        {
            ItemManager.Instance.CheckConsumable(consumableIndex);
            UIManager.instance.UpdateObjectInfo();
            Destroy(gameObject);
        }
        
        public override void OnAcquire(bool inShop)
        {
            ItemManager.Instance.CheckConsumable(consumableIndex);
            UIManager.instance.UpdateObjectInfo();
        }
    }
}
