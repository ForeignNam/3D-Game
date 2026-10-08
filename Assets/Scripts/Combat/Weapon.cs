using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
    public class Weapon : ScriptableObject
    {
         [SerializeField] float weaponRange;
         [SerializeField] float weaponDamage ;
        [SerializeField] private GameObject weaponPrefab = null;
        [SerializeField] private AnimatorOverrideController weaponanimation = null;
        [SerializeField] private bool isRightHand = true;

        public void Spawn(Transform rightHandTransform,Transform leftHandTransform, Animator animator)
        { 
            Transform handTransform = isRightHand ? rightHandTransform : leftHandTransform;
            if(weaponPrefab != null)
            {
               Instantiate(weaponPrefab, handTransform); 
            }
              
            if(weaponanimation != null)
            {
                animator.runtimeAnimatorController = weaponanimation;
            }
            
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
