using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
namespace RPG.Control
{
    [RequireComponent(typeof(Fighter), typeof(Movers), typeof(Health))]
    public class PlayerController : MonoBehaviour
    {
        private Fighter fighter;
        private Movers movers;
        private Health health;

        private void Awake()
        {
            CacheComponents();
        }

        private void OnEnable()
        {
            CacheComponents();
        }

        private void CacheComponents()
        {
            movers = GetComponent<Movers>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();

            if (movers == null || fighter == null || health == null)
            {
                Debug.LogError(
                    $"{name} requires Movers, Fighter, and Health on the same GameObject.",
                    this);
                enabled = false;
            }
        }

        private void Update()
        {
            if (movers == null || fighter == null || health == null)
            {
                CacheComponents();
                if (!enabled) return;
            }

            if(health.Dead()) return;
          if(InteractWithCombat()) return;
          if(InteractWithMouse()) return;
       


        }
         private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.collider.GetComponentInParent<CombatTarget>();
                if (target == null)
                {
                    continue;
                }
                
                if (!fighter.CanAttack(target.gameObject))
                {
                    continue;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    fighter.Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }
         

        private bool InteractWithMouse()
        {                  
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                  if (Input.GetMouseButton(0))
                  {
                      GetComponent<Fighter>().Cancel();
                      movers.MoveTo(hit.point, 1f);
                  }
                
                return true;

            }
            return false;

        }
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

    }
}
