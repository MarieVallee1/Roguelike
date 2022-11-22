using System;
using UnityEngine;

namespace Objects
{
    public class ShopSlot : MonoBehaviour
    {
        [SerializeField] private bool isHealth;
        private Reward _linkedReward;
        [SerializeField] private Animator anim;
        private void Start()
        {
            if (!isHealth)
            {
                //_linkedReward = ItemManager.Instance.PickItem();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            //Show on Ui name + description
            anim.SetBool("isOpen",true);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            //Mask on Ui name + description
            anim.SetBool("isOpen",false);
        }

        private void BuyItem()
        {
            if (GameManager.instance.pearlAmount <= _linkedReward.GetPrice()) return;
            GameManager.instance.pearlAmount -= _linkedReward.GetPrice();
            _linkedReward.isOwned = true;
            _linkedReward.OnAcquire();
            gameObject.SetActive(false);
        }
    }
}
