using UnityEngine;
using RPG.Core;
namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Scriptable Objects/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] float weaponRange;
        [SerializeField] float weaponDamage;
        [SerializeField] private Weapon weaponPrefab = null;
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
                Weapon weapon = Instantiate(weaponPrefab, handTransform);
                weapon.gameObject.name = WEAPON;
            }
            var overrideAnimator = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (weaponanimation != null)
            {
                animator.runtimeAnimatorController = weaponanimation;
            }
            else if (overrideAnimator != null)
            {
                animator.runtimeAnimatorController = overrideAnimator.runtimeAnimatorController;
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
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {

            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target,weaponDamage, instigator);
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
