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
        [SerializeField] private PatrolPath patrolPath;
        private Movers movers;
        private Fighter fighter;
        private ActionSchedule actionSchedule;
        private Health health;
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceEnemyArriveWayPoints = Mathf.Infinity;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private float stopWaypoint = 2f;
        private int currentWaypointindex = 0;
        private float defaultdistance = 1f;
        [Range(0,1)]
        [SerializeField] private float patrolspeedFraction = 0.2f;
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
                
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceEnemyArriveWayPoints += Time.deltaTime;
        }
        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }
        private void SuspicionBehaviour()
        {
            actionSchedule.CancelCurrentAction();
        }
        private void PatrolBehaviour()
        {
            Vector3 nextWaypoint = guardPosition;
            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceEnemyArriveWayPoints =0;
                    CircleWayPoint();
                }
                nextWaypoint = GetCurrentWayPoint();
            }

            if(timeSinceEnemyArriveWayPoints > stopWaypoint)
            {
                movers.StartMoveAction(nextWaypoint, patrolspeedFraction);
            }
            
        }

        private bool AtWayPoint()
        {
            float distanceWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceWaypoint < defaultdistance;
        }
        private void CircleWayPoint()
        {
            currentWaypointindex = patrolPath.GetNextIndex(currentWaypointindex);
        }
        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWayPoint(currentWaypointindex);
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