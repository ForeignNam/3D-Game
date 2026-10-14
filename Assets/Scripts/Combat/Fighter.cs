using UnityEngine;
using UnityEngine.Serialization;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        
        
        [SerializeField] float timebetweenattacks = 2f;
        Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private ActionSchedule actionSchedule;
        private Movers movers;
        private Animator anim;

        [FormerlySerializedAs("weapon")]
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
            if (currentWeapon == null)
            {
                Debug.LogWarning($"{name} does not have a weapon to equip.", this);
                return;
            }

            defaultweapon = currentWeapon;
            currentWeapon.Spawn(rightHandTransform, leftHandTransform, anim);
        }

        public object CaptureState()
        {
            return defaultweapon != null ? defaultweapon.name : string.Empty;
        }
        public Health GetTarget()
        {
            return target;
        }
        public void RestoreState(object state)
        {
            string weaponName = state as string;
            if (string.IsNullOrWhiteSpace(weaponName)) return;

            Weapon weapon = Resources.Load<Weapon>(weaponName);
            if (weapon == null)
            {
                Debug.LogWarning(
                    $"Could not restore weapon '{weaponName}' from a Resources folder.",
                    this);
                return;
            }

            EquipWeapon(weapon);
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
            if (defaultweapon.HasProjectile())
            {
                defaultweapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else
            {
                target.TakeDamage(defaultweapon.GetWeaponDamage());
            }

        }
        void Shoot()
        {

            Hit();
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
