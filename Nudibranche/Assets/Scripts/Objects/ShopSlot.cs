using System;
using Character;
using TMPro;
using UnityEngine;

namespace Objects
{
    public class ShopSlot : MonoBehaviour
    {
        [SerializeField] private bool isHealth;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator anim;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        [SerializeField] private TextMeshProUGUI itemPrice;
        
        
        private Reward _linkedReward;
        private bool _inZone;
        private void Start()
        {
            _linkedReward = isHealth ? ItemManager.Instance.health : ItemManager.Instance.PickItem();
            spriteRenderer.sprite = _linkedReward.stats.objectImage;
            itemName.text = _linkedReward.stats.objectName;
            itemDescription.text = _linkedReward.stats.objectDescription;
            itemPrice.text = _linkedReward.stats.objectPrice + "";
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer != 6) return;
            _inZone = true;
            //Show on Ui name + description
            anim.SetBool("isOpen",true);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer != 6) return;
            _inZone = false;
            //Mask on Ui name + description
            anim.SetBool("isOpen",false);
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
                if (isHealth) BuyHealth();
                else BuyItem();
            }
        }
        
        private void BuyHealth()
        {
            if (GameManager.instance.pearlAmount <= _linkedReward.GetPrice()) return;
            GameManager.instance.pearlAmount -= _linkedReward.GetPrice();
            var player = PlayerController.Instance;
            var maxHealth = player.characterData.health;
            if (player.health < maxHealth) _linkedReward.OnAcquire();
        }
        
        private void BuyItem()
        {
            if (GameManager.instance.pearlAmount < _linkedReward.GetPrice()) return;
            GameManager.instance.pearlAmount -= _linkedReward.GetPrice();
            _linkedReward.isOwned = true;
            if(_linkedReward.stats.consumable) _linkedReward.OnAcquire(true);
            else _linkedReward.OnAcquire();
            gameObject.SetActive(false);
        }
    }
}
