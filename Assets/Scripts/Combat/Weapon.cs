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

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            Transform handTransform = GetTransform(rightHandTransform, leftHandTransform);
            if (weaponPrefab != null)
            {
                Instantiate(weaponPrefab, handTransform);
            }

            if (weaponanimation != null)
            {
                animator.runtimeAnimatorController = weaponanimation;
            }

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
