using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class LayoutGenerator : MonoBehaviour
{
    public Object[] salle;
    private int _test;
    private Vector3 _newPos;
    void Start()
    {
        salle = Resources.LoadAll("StartRoom", typeof(GameObject));

        _test = 0;
        var salle1 = salle[_test].GameObject();
        Instantiate(salle1,transform,instantiateInWorldSpace:true);
        
        salle = Resources.LoadAll("3Room/SEO", typeof(GameObject));
        var salle2 = salle[_test].GameObject();
        _newPos = new Vector3(0, salle1.transform.lossyScale.y / 2 + salle2.transform.lossyScale.y / 2, 0);
        Instantiate(salle2,_newPos,quaternion.identity,transform);
    }
}
