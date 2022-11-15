using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class SkillsDetails : MonoBehaviour
{
    [Header("Ability Cooldown")]

    private List<EnemyHealth> _enemiesInSight;

    public IEnumerator SwordSlash()
    {
        PlayerController.instance.isUsingSkill = true;
        
        var enemyDetection = EnemyDetection.instance;
        
        enemyDetection.enabled = true;
        StartCoroutine(UIManager.instance.ScieRanoSlash());
        yield return new WaitForSeconds(0.5f);
        
        _enemiesInSight = EnemyDetection.instance.enemiesInSight;
        enemyDetection.enabled = false;
        yield return new WaitForSeconds(0.5f);
        
        for (int i = 0; i < _enemiesInSight.Count; i++)
        {
            _enemiesInSight[i].takeDamage(3);
        }
        yield return new WaitForSeconds(2f);
        
        PlayerController.instance.isUsingSkill = false;
        
        yield return new WaitForSeconds(1f);
        
       
    }
    public void WrongTrack()
    {
        //Compétence Shellock
    }
    public void CardLaser()
    {
        //Compétence Sirène
    }
}
