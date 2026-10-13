using UnityEngine;
using RPG.Core;
namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
    public class Weapon : ScriptableObject
    {
        [SerializeField] float weaponRange;
        [SerializeField] float weaponDamage;
        [SerializeField] private GameObject weaponPrefab = null;
        [SerializeField] private AnimatorOverrideController weaponanimation = null;
        [SerializeField] private bool isRightHand = true;
        [SerializeField] private Projectile projectile = null;
        private const string WEAPON = "Weapon";

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
             DestroyOldWeapon(rightHandTransform,leftHandTransform);
            Transform handTransform = GetTransform(rightHandTransform, leftHandTransform);
            if (weaponPrefab != null)
            {
                GameObject weapon = Instantiate(weaponPrefab, handTransform);
                weapon.name = WEAPON;
            }

            if (weaponanimation != null)
            {
                animator.runtimeAnimatorController = weaponanimation;
            }

        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(WEAPON);
            if(oldWeapon==null) oldWeapon = leftHand.Find(WEAPON);
            if(oldWeapon==null) return;

            oldWeapon.name = "Destroying";
            Destroy(oldWeapon.gameObject);
            
            
        }
        public bool HasProjectile()
        {
            return projectile != null;
        }
        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform = isRightHand ? rightHand : leftHand;
            return handTransform;

        }
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {

            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target,weaponDamage);
        }



        public float GetWeaponRange()
        {
            return weaponRange;
        }
        public float GetWeaponDamage()
        {
            return weaponDamage;
        }


    }
}
