using System;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class Avarice : Reward
    {
        [SerializeField] private int pearlBonus = 1;

        public override void OnAcquire()
        {
            Debug.Log("Avarice équipée");
            GameManager.instance.moulePearlDrop += pearlBonus;
            GameManager.instance.crevettePearlDrop += pearlBonus;
            GameManager.instance.canonnierPearlDrop += pearlBonus;
        }
    }
}
