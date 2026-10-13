using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Fighter fighter;
        private Movers movers;
        private Health health;
        private void Awake()
        {
            movers = GetComponent<Movers>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if(health.Dead()) return;
          if(InteractWithCombat()) return;
          if(InteractWithMouse()) return;
       


        }
         private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            if(Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Click detected, hits count: " + hits.Length);
            }

            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.collider.GetComponentInParent<CombatTarget>();
                if(target == null)
                {
                    if(Input.GetMouseButtonDown(0))
                        //Debug.Log("Hit object without CombatTarget: " + hit.collider.gameObject.name);
                    continue;
                }
                
                if(!fighter.CanAttack(target.gameObject))
                {
                    if(Input.GetMouseButtonDown(0))
                        //Debug.Log("Cannot attack: " + target.gameObject.name);
                    continue;
                }
                if(Input.GetMouseButtonDown(0))
                {
                    //Debug.Log("Attacking: " + target.gameObject.name);
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
