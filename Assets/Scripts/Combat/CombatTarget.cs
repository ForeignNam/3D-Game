using UnityEngine;
using RPG.Core;
using RPG.Control;
namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
      public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(this.gameObject);
            }
            return true;
        }
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }
}