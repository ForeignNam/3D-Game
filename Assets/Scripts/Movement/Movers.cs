using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
namespace RPG.Movement
{
    public class Movers : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Transform target;
        private Animator animator;
        private ActionSchedule actionSchedule;
        private Health health;
        NavMeshAgent navMeshAgent;
        [SerializeField] private float maxSpeed = 6f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            actionSchedule = GetComponent<ActionSchedule>();
            health = GetComponent<Health>();
        }
        void Update()
        {
            navMeshAgent.enabled = !health.Dead();
            UpdateAnimator();

        }
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            actionSchedule.StartAction(this);
            MoveTo(destination,speedFraction);
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * speedFraction;
            navMeshAgent.isStopped = false;
        }
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }
        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            navMeshAgent.enabled = false;
            transform.position = position.ToVector();
            navMeshAgent.enabled = true;
        }
    }
}