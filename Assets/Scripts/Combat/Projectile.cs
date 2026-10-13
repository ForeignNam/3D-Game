using UnityEngine;
using RPG.Core;
namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        Health target;
        
        [SerializeField] private float speed;
        [SerializeField] GameObject impactFX;
        
        [SerializeField] private float damage;
        [SerializeField] private bool isHoming = false ;
        private void Awake()
        {
            Rigidbody projectileRigidbody = GetComponent<Rigidbody>();
            if (projectileRigidbody == null)
            {
                projectileRigidbody = gameObject.AddComponent<Rigidbody>();
            }

            projectileRigidbody.useGravity = false;
            projectileRigidbody.isKinematic = true;
            projectileRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        private void Start()
        {
            transform.LookAt(GetlocationTarget());
        }

        // Update is called once per frame
        void Update()
        {
            if (target != null)
            {
                if(isHoming && !target.Dead())
                {
                     transform.LookAt(GetlocationTarget());
                }
               
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;

        }

        private void OnTriggerEnter(Collider other)
        {
            Health hitHealth = other.GetComponentInParent<Health>();
            if (hitHealth == null || hitHealth != target) return;
            if(target.Dead()) return;   
            if (impactFX != null)
            {
                GameObject impactInstance = Instantiate(impactFX, GetlocationTarget(), Quaternion.identity);
                if (impactInstance.GetComponent<Destroybullet>() == null)
                {
                    impactInstance.AddComponent<Destroybullet>();
                }
            }
            hitHealth.TakeDamage(damage);
            Destroy(gameObject); 
        }

        private Vector3 GetlocationTarget()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            if (targetCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCollider.height / 2;
        }


    }
}
