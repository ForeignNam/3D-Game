using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float AIradius = 5f;
        private GameObject player;
        private Movers movers;
        private Fighter fighter;
        private ActionSchedule actionSchedule;
        private Health health;
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        [SerializeField] private float suspicionTime = 3f;
        void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            guardPosition = transform.position;
            movers = GetComponent<Movers>();
            actionSchedule = GetComponent<ActionSchedule>();
        }
        void Update()
        {
            if (health.Dead()) return;
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                fighter.Attack(player);
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                actionSchedule.CancelCurrentAction();
            }
            else
            {
                movers.StartMoveAction(guardPosition);
            }
            timeSinceLastSawPlayer += Time.deltaTime;
        }
        private bool InAttackRangeOfPlayer()
        {
            float v = Vector3.Distance(transform.position, player.transform.position);
            return v < AIradius;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AIradius);
        }

    }
}