using System;
using UnityEngine;

namespace Objects
{
    public class ShopSlot : MonoBehaviour
    {
        [SerializeField] private bool isHealth;
        private Reward _linkedReward;
        private void Start()
        {
            if (!isHealth)
            {
                _linkedReward = ItemManager.Instance.PickItem();
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //Show on Ui name + description
            throw new NotImplementedException();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            //Mask on Ui name + description
            throw new NotImplementedException();
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
