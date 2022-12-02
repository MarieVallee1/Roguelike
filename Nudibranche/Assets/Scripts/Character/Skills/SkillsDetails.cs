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
        [HideInInspector]public float cooldownReduction = 1;
        private List<EnemyHealth> _enemiesInSight;
        private List<RaycastHit2D> _hit;
        private ContactFilter2D _filter;
    
        [SerializeField] private GameObject bait;

        private void Awake()
        { 
            _hit = new ();
            _filter = new ContactFilter2D();
            _filter.SetLayerMask(LayerMask.GetMask("Moule","Crevette","Canonnier"));
        }

        private void Update()
        {
            Debug.DrawRay(PlayerController.Instance.characterPos,(PlayerController.Instance.aim.normalized)* 35f);
        }

        public IEnumerator SwordSlash()
        {
            PlayerController.Instance.FreezeCharacter();
            PlayerController.Instance.DisableInputs();
            PlayerController.Instance.onSkillUse = true;
        
            var enemyDetection = EnemyDetection.instance;
        
            enemyDetection.enabled = true;
            StartCoroutine(UIManager.instance.ScieRanoSlash());
            yield return new WaitForSeconds(0.5f);
        
            _enemiesInSight = EnemyDetection.instance.enemiesInSight;
            enemyDetection.enabled = false;
            yield return new WaitForSeconds(0.5f);
        
            for (int i = 0; i < _enemiesInSight.Count; i++)
            {
                _enemiesInSight[i].takeDamage(PlayerController.Instance.characterData.swordSlashDamages);
            }
            yield return new WaitForSeconds(2f);
        
            PlayerController.Instance.onSkillUse = false;
            PlayerController.Instance.skillCooldown = PlayerController.Instance.characterData.swordSlashCooldown/cooldownReduction;
        
            PlayerController.Instance.UnfreezeCharacter();
            PlayerController.Instance.EnableInputs();
            PlayerController.Instance.skillCountdown = 0;
        }
        public void WrongTrack(Vector3 playerPos)
        {
            string baitRef = bait.name;
            GameObject usedProjectile = PoolingSystem.instance.GetObject(baitRef);
       
            if (usedProjectile != null)
            { 
                //Placement & activation
                usedProjectile.transform.position = playerPos;
                usedProjectile.SetActive(true);
           
                PlayerController.Instance.skillCooldown = PlayerController.Instance.characterData.baitCooldown/cooldownReduction;
                PlayerController.Instance.skillCountdown = 0;
            }
        }
        public IEnumerator CardLaser(Vector3 bookPos, Vector2 dir)
        { 
            PlayerController.Instance.FreezeCharacter();
            PlayerController.Instance.DisableInputs();
            
            Physics2D.BoxCast(PlayerController.Instance.characterPos, new Vector2(3,3), BookPosition.Instance.directionAngle,PlayerController.Instance.aim, _filter, _hit);


            for (int i = 0; i < _hit.Count; i++)
            {
                _hit[i].transform.GetComponent<EnemyHealth>().takeDamage(PlayerController.Instance.characterData.cardLaserDamages);
            }

            yield return new WaitForSeconds(2);
        
            PlayerController.Instance.UnfreezeCharacter();
            PlayerController.Instance.EnableInputs();
        
            PlayerController.Instance.skillCooldown = PlayerController.Instance.characterData.cardLaserCooldown/cooldownReduction;
            PlayerController.Instance.skillCountdown = 0;
        }
    }
}
