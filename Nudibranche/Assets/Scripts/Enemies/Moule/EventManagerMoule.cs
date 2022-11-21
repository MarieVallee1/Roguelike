using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManagerMoule : MonoBehaviour
{
    [SerializeField] private IAMoule iaMoule;
    
    public void InflictDamages()
    {
        iaMoule.InflictDamages();
    }

    public void AttackEnd()
    {
        iaMoule.AttackEnded();
    }
}
