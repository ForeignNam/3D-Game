using UnityEngine;
using RPG.Saving;
using RPG.Stats;
 namespace RPG.Core
 {   
    public class Health : MonoBehaviour, ISaveable
    {
        private Animator anim;
        private bool isdead = false;
        private ActionSchedule actionSchedule;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            actionSchedule = GetComponent<ActionSchedule>();
        }
        public bool Dead()
        {
            return isdead;
        }

        public float GetHealthPoints()
        {
            return healthPoint;
        }

        [SerializeField] float healthPoint = 100f;
        private void Start()
        {
            healthPoint = GetComponent<BaseStats>().GetHealth();
        }
        public void TakeDamage(float damage)
        {
            healthPoint = Mathf.Max(healthPoint - damage,0);
            if(healthPoint == 0) Death();
           
        }

        public void Death()
        {
        if(isdead) return;
        isdead = true;
        anim.SetTrigger("die");
        actionSchedule.CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoint;
        }
        public void RestoreState(object state)
        {
            healthPoint = (float)state;
            if(healthPoint == 0) Death();
        }
    }
}
