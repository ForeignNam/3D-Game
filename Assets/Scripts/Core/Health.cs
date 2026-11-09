using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Attributes;
using UnityEngine.Events;
 namespace RPG.Core
 {   
    public class Health : MonoBehaviour, ISaveable
    {
        private Animator anim;
        private bool isdead = false;
        private ActionSchedule actionSchedule;
        private BaseStats baseStats;
        [SerializeField] TakeDamageEvent onTakeDamage;
        [SerializeField] UnityEvent onDie;
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>{}
        private void Awake()
        {
            anim = GetComponent<Animator>();
            actionSchedule = GetComponent<ActionSchedule>();
            baseStats = GetComponent<BaseStats>();
        }
        public bool Dead()
        {
            return isdead;
        }
       
        public float GetPercentage()
        {
            return 100 * GetFraction(); }
        public float GetFraction()
        {
            return healthPoint / baseStats.GetStat(Stat.health);
        }
        [SerializeField] float healthPoint = 100f;
        private void Start()
        {
            healthPoint = baseStats.GetStat(Stat.health);
        }
        public void TakeDamage(float damage, GameObject Integator)
        {
            healthPoint = Mathf.Max(healthPoint - damage,0);
            if(healthPoint == 0)
            {
               onDie.Invoke();
                Death();
                AwardExperience(Integator);
            } 
            else
            {
                onTakeDamage.Invoke(damage);
            }

           
        }
        private void AwardExperience(GameObject integator)
        {
            Experience experience = integator.GetComponent<Experience>();
            if(experience != null)
            {
                
                experience.GainExperience(baseStats.GetStat(Stat.experience));
            }
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
