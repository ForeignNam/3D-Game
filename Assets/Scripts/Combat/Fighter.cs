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
        private float timeSinceLastAttack = Mathf.Infinity;
        private ActionSchedule actionSchedule;
        private Animator anim;
        private void Awake()
        {
            actionSchedule = GetComponent<ActionSchedule>();
            anim = GetComponent<Animator>();
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.Dead()) return;
            bool targetinrange = Vector3.Distance(transform.position, target.transform.position) < weaponRange;
            if (target != null && !targetinrange)
            {
                GetComponent<Movers>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Movers>().Cancel();
                AttackBehaviour();
            }

        }
        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timebetweenattacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }

        }
        private void TriggerAttack()
        {
            anim.ResetTrigger("stopAttack");
            anim.SetTrigger("attack");
        }
        public bool CanAttack(GameObject combattarget)
        {
            if (combattarget == null) return false;
            Health targethealth = combattarget.GetComponent<Health>();
            return targethealth != null && !targethealth.Dead();
        }
        public void Hit()
        {
            if(target==null) return;
            target.TakeDamage(10f);
        }

        public void Attack(GameObject combattarget)
        {
            actionSchedule.StartAction(this);
            target = combattarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            anim.ResetTrigger("attack");
            anim.SetTrigger("stopAttack");
            
        }
    }
}