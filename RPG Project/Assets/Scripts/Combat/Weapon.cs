using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName="Weapon", menuName="Weapons/Make new weapon", order=0)]
        
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] AnimatorOverrideController overrideController = null;
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] float weaponRange = 2;
        [SerializeField] bool rightHanded = true;   
        [SerializeField] Projectile projectile = null;
        const string weaponName = "Weapon";
        public float GetDamage() { return weaponDamage; }
        public float GetRange() { return weaponRange; }
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator){
            DestroyOldWeapon(rightHand, leftHand);
            if(weaponPrefab != null){
                GameObject weapon = Instantiate(weaponPrefab, GetTransform(rightHand, leftHand));
                weapon.name = weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if(overrideController != null){
                animator.runtimeAnimatorController = overrideController;
            }else if(overrideController != null){
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        void DestroyOldWeapon(Transform rightHand, Transform leftHand){
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null){
                oldWeapon = leftHand.Find(weaponName);
            }
            if(oldWeapon == null){
                return;
            }
            oldWeapon.name = "";
            Destroy(oldWeapon.gameObject);
        }

        public bool HasProjectile(){
            return projectile != null;
        }
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator){
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage, instigator);
        }

        Transform GetTransform(Transform rightHand, Transform leftHand){
            return rightHanded ? rightHand : leftHand;
        }        
    }
}
