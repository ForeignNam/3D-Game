using UnityEngine;
using RPG.Movement;
using RPG.Combat;
namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Movers movers;
        private void Awake()
        {
            movers = GetComponent<Movers>();
        }

        private void Update()
        {

          if(InteractWithCombat()) return;
          if(InteractWithMouse()) return;
       


        }
         private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.collider.GetComponent<CombatTarget>();
                if(target == null) continue;
                if(Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target);
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
                      movers.MoveTo(hit.point);
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
