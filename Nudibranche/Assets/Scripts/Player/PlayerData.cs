using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public float characterSpeed = 20f;
    public float basicAttackSpeed = 20f;
    public float basicAttackCooldown = 0.5f;

    public int hello;
}