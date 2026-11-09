using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using UnityEngine.AI;
namespace RPG.Control
{
    [RequireComponent(typeof(Fighter), typeof(Movers), typeof(Health))]
    public class PlayerController : MonoBehaviour
    {
        private Fighter fighter;
        private Movers movers;
        private Health health;
     
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;

        }
        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] private float maxNavMeshProjectionDistance = 1f;
        [SerializeField] private float maxNavMeshPathDistance = 40f;
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
            if(InteractWithUI()) return;
            if (movers == null || fighter == null || health == null)
            {
                CacheComponents();
                if (!enabled) return;
            }

            if (health.Dead())
            {
                SetCursor(CursorType.None);
                return;
            }
          if (InteractWithCombat()) return;
          if (InteractwithComponent()) return;
          if (InteractWithMouse()) return;
          SetCursor(CursorType.None);



        }
        private bool InteractWithUI()
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
            ;
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
                SetCursor(CursorType.Combat);
                return true;
            }
            return false;
        }
        public bool InteractwithComponent()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastable = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable iraycastable in raycastable)
                {
                    if(iraycastable.HandleRaycast(this))
                    {
                        SetCursor(iraycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }
        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit,
             maxNavMeshProjectionDistance, NavMesh.AllAreas);
             if (!hasCastToNavMesh) return false;
            target = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if(GetNavMeshPathLength(path) > maxNavMeshPathDistance) return false;
            return true;
        }
        private float GetNavMeshPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return total;
        }
        private bool InteractWithMouse()
        {                  
            //RaycastHit hit;
            //bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (!hasHit) return false;
            if (hasHit)
            {
                  if (Input.GetMouseButton(0))
                  {
                      GetComponent<Fighter>().Cancel();
                      movers.MoveTo(target, 1f);
                  }
                SetCursor(CursorType.Movement);
                return true;

            }
            return false;

        }
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);

        }
       private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }   
    }
}
