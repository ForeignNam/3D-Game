using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Combat;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timebetweenattacks = 2f;
        Health target;
        private float timeSinceLastAttack = 0;
        private ActionSchedule actionSchedule;
        private Animator ani;
        private void Awake()
        {
            actionSchedule = GetComponent<ActionSchedule>();
            ani = GetComponent<Animator>();
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if(target.Dead()) return;
            bool targetinrange = Vector3.Distance(transform.position, target.transform.position) < weaponRange;
            if (target != null && !targetinrange)
            {
                GetComponent<Movers>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Movers>().Cancel();
                AttackBehaviour();
            }

        }
        private void AttackBehaviour()
        {
            if (timeSinceLastAttack >= timebetweenattacks)
            {
                ani.SetTrigger("attack");
                timeSinceLastAttack = 0;
            }

        }
        public void Hit()
        {
           
               target.TakeDamage(10f);
            }

        public void Attack(CombatTarget combattarget)
        {
            actionSchedule.StartAction(this);
            target = combattarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
        }
    }
}