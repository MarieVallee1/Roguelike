using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Character.Skills
{
    public class SkillsDetails : MonoBehaviour
    {
        [Header("Ability Cooldown")]
        public float swordSlashCooldown;
        public float wrongTrackCooldown;
        public float cardLaserCooldown;
        private List<EnemyHealth> _enemiesInSight;
    
        [SerializeField] private GameObject bait;
    
        [SerializeField] private LineRenderer laserBeam;
    

        public IEnumerator SwordSlash()
        {
            PlayerController.instance.FreezeCharacter();
            PlayerController.instance.DisableInputs();
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
            PlayerController.instance.skillCooldown = swordSlashCooldown;
        
            PlayerController.instance.UnfreezeCharacter();
            PlayerController.instance.EnableInputs();
        }
        public void WrongTrack(Vector3 playerPos)
        { 
            PlayerController.instance.FreezeCharacter();
            PlayerController.instance.DisableInputs();
            string baitRef = bait.name;
            GameObject usedProjectile = PoolingSystem.instance.GetObject(baitRef);
       
            if (usedProjectile != null)
            { 
                //Placement & activation
                usedProjectile.transform.position = playerPos;
                usedProjectile.SetActive(true);
           
                PlayerController.instance.skillCooldown = wrongTrackCooldown;
            }
            PlayerController.instance.UnfreezeCharacter();
            PlayerController.instance.EnableInputs();
        }
        public IEnumerator CardLaser(Vector3 bookPos, Vector2 dir)
        { 
            PlayerController.instance.FreezeCharacter();
            PlayerController.instance.DisableInputs();

            RaycastHit2D hit;
            hit = Physics2D.Raycast(bookPos, dir, 30);
        
            laserBeam.SetPosition(0,bookPos);
            laserBeam.SetPosition(1,(Vector2)bookPos + dir);
        
            yield return new WaitForSeconds(2);
        
            PlayerController.instance.UnfreezeCharacter();
            PlayerController.instance.EnableInputs();
        
            PlayerController.instance.skillCooldown = cardLaserCooldown;
        }
    }
}
