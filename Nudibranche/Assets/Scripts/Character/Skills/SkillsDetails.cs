using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using GenPro;
using UnityEngine;

namespace Character.Skills
{
    public class SkillsDetails : MonoBehaviour
    {
        [HideInInspector]public float cooldownReduction = 1;
        private List<EnemyHealth> _enemiesInSight;
        private List<RaycastHit2D> _hit;
        private ContactFilter2D _filter;
    
        [SerializeField] private GameObject bait;
        [SerializeField] private  ParticleSystem cardLaserVFX;

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
            PlayerController.Instance.vulnerable = false;
        
            var enemyDetection = EnemyDetection.instance;
        
            enemyDetection.enabled = true;
            StartCoroutine(UIManager.instance.ScieRanoSlash());
            
            yield return new WaitForSeconds(0.5f);
            
            AudioList.Instance.PlayOneShot(AudioList.Instance.scierano,AudioList.Instance.scieranoVolume);

            _enemiesInSight = EnemyDetection.instance.enemiesInSight;
            enemyDetection.enabled = false;
            yield return new WaitForSeconds(0.5f);
            AudioList.Instance.PlayOneShot(AudioList.Instance.mouleSword,AudioList.Instance.scieranoVolume);

            if (GameManager.instance.bossFightOn)
            {
                GameManager.instance.bossScript.TakeDamage(PlayerController.Instance.characterData.swordSlashDamages);
            }
            else
            {
                for (int i = 0; i < _enemiesInSight.Count; i++)
                {
                    _enemiesInSight[i].takeDamage(PlayerController.Instance.characterData.swordSlashDamages);
                }
            }

            yield return new WaitForSeconds(0.4f);
            AudioList.Instance.PlayOneShot(AudioList.Instance.scieranoEnd,AudioList.Instance.scieranoEndVolume);
            yield return new WaitForSeconds(0.2f);
        
            PlayerController.Instance.onSkillUse = false;
            PlayerController.Instance.skillCooldown = PlayerController.Instance.characterData.swordSlashCooldown/cooldownReduction;
        
            PlayerController.Instance.UnfreezeCharacter();
            PlayerController.Instance.EnableInputs();
            PlayerController.Instance.skillCountdown = 0;
            PlayerController.Instance.vulnerable = true;
        }
        public void WrongTrack(Vector3 playerPos)
        {
            AudioList.Instance.PlayOneShot(AudioList.Instance.shellock,AudioList.Instance.shellockVolume);
            
            string baitRef = bait.name;
            GameObject usedProjectile = PoolingSystem.instance.GetObject(baitRef);
            Transform baitTransform;
       
            if (usedProjectile != null)
            { 
                //Placement & activation
                usedProjectile.transform.position = playerPos;
                usedProjectile.SetActive(true);
                baitTransform = usedProjectile.transform;
           
                PlayerController.Instance.skillCooldown = PlayerController.Instance.characterData.baitCooldown/cooldownReduction;
                PlayerController.Instance.skillCountdown = 0;
                
                RoomManager currentRoom;
                currentRoom = GameManager.instance.currentRoom;

                if (currentRoom != null)
                {
                    List<ActivateEnemy> scriptList = new();
                    scriptList = currentRoom._enemyList;

                    for (int i = 0; i < scriptList.Count; i++)
                    {
                        scriptList[i].BaitDetection(baitTransform);
                    } 
                }
            }
        }
        public IEnumerator CardLaser(Vector3 bookPos, Vector2 dir)
        { 
            AudioList.Instance.PlayOneShot(AudioList.Instance.basicAttack,AudioList.Instance.sireineDeCoeurVolume*0.75f);
            
            cardLaserVFX.Play();
            BookPosition.Instance.onLaserCardUse = true;
            PlayerController.Instance.vulnerable = false;
            PlayerController.Instance.FreezeCharacter();
            PlayerController.Instance.DisableInputs();
            
            yield return new WaitForSeconds(1);
            
            AudioList.Instance.PlayOneShot(AudioList.Instance.sireineDeCoeur,AudioList.Instance.sireineDeCoeurVolume);

            Physics2D.BoxCast(PlayerController.Instance.characterPos, new Vector2(3,3), BookPosition.Instance.directionAngle,PlayerController.Instance.aim, _filter, _hit);

            if (GameManager.instance.bossFightOn)
            {
                GameManager.instance.bossScript.TakeDamage(PlayerController.Instance.characterData.cardLaserDamages);
            }
            else
            {
                for (int i = 0; i < _hit.Count; i++)
                {
                    var script = _hit[i].transform.GetComponent<EnemyHealth>();
                    if(script.projectileHit.enabled) 
                        script.takeDamage(PlayerController.Instance.characterData.cardLaserDamages);
                }
            }

            yield return new WaitForSeconds(2);
        
            PlayerController.Instance.UnfreezeCharacter();
            PlayerController.Instance.EnableInputs();
            BookPosition.Instance.onLaserCardUse = false;

            PlayerController.Instance.skillCooldown = PlayerController.Instance.characterData.cardLaserCooldown/cooldownReduction;
            PlayerController.Instance.skillCountdown = 0;
            
            yield return new WaitForSeconds(3);

            PlayerController.Instance.vulnerable = true;
        }
    }
}
