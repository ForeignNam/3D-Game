using UnityEngine;
namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {
        public void TakeDamage(float damage)
        {
            Debug.Log(damage);
        }
    }
}