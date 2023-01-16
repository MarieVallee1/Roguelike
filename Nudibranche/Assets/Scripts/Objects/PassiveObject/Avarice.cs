using System;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class Avarice : Reward
    {
        [SerializeField] private float redPearlBonus = 0.3f;

        public override void OnAcquire()
        {
            GameManager.instance.mouleLifeDrop += redPearlBonus;
            GameManager.instance.crevetteLifeDrop += redPearlBonus;
            GameManager.instance.canonnierLifeDrop += redPearlBonus;
        }
    }
}
