using UnityEngine;
using RPG.Movement;
using RPG.Core;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {


        [SerializeField] float timebetweenattacks = 2f;
        Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private ActionSchedule actionSchedule;
        private Movers movers;
        private Animator anim;

        [SerializeField] private Weapon defaultweapon = null;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;


        private void Awake()
        {
            actionSchedule = GetComponent<ActionSchedule>();
            movers = GetComponent<Movers>();
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            EquipWeapon(defaultweapon);
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.Dead()) return;
            bool targetinrange = Vector3.Distance(transform.position, target.transform.position) < defaultweapon.GetWeaponRange();
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
        public void EquipWeapon(Weapon currentWeapon)
        {
            if (currentWeapon == null) return;
            currentWeapon.Spawn(rightHandTransform, leftHandTransform, anim);
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
            if (target == null) return;
            target.TakeDamage(defaultweapon.GetWeaponDamage());
        }

        public void Attack(GameObject combattarget)
        {
            actionSchedule.StartAction(this);
            target = combattarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            movers.Cancel();
            target = null;
        }

        private void StopAttack()
        {
            anim.ResetTrigger("attack");
            anim.SetTrigger("stopAttack");

        }
    }
}
