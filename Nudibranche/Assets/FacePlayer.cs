using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
   [SerializeField] private GameObject face;
   [SerializeField] private GameObject dos;
   [SerializeField] private GameObject profil;
   [SerializeField] private Transform target;

   private void Start()
   {
      target = PlayerController.instance.transform;
   }
}
