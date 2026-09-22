using UnityEngine;
 namespace RPG.Combat
 {   
    public class Health : MonoBehaviour
    {
        private Animator anim;
        private bool isdead = false;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        public bool Dead()
        {
            return isdead;
        }
        [SerializeField] float healthPoint = 100f;
        public void TakeDamage(float damage)
        {
            healthPoint = Mathf.Max(healthPoint - damage,0);
            if(healthPoint == 0) Death();
           
        }

        private void Death()
        {
        if(isdead) return;
        isdead = true;
        anim.SetTrigger("die");
        }
    }
}