using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects;

[CreateAssetMenu(fileName = "Data", menuName = "Perle")]

public class Perle : ScriptableObject
{
    public void LootDrop(Vector2 spawnPos)
    {
        GameObject usedPearl = PoolingSystem.instance.GetObject(perleName);

        if (usedPearl != null)
        {
            //Placement & activation
            usedPearl.transform.position = spawnPos;
            usedPearl.SetActive(true);
        }
    }
    
    [Header("Pearl Type")] 
    public string perleName;
    public string userName;
    [TextArea] public string description;

    [Header("Characteristics")] 
    public bool monney;
}
