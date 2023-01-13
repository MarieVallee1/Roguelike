using System;
using UnityEngine;

namespace Objects.PassiveObject
{
    public class Avarice : Reward
    {
        [SerializeField] private int redPearlBonus = 1;

        public override void OnAcquire()
        {
            GameManager.instance.mouleLifeDrop += redPearlBonus;
            GameManager.instance.crevetteLifeDrop += redPearlBonus;
            GameManager.instance.canonnierLifeDrop += redPearlBonus;
        }
    }
}
