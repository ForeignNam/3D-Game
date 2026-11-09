using UnityEngine;
using RPG.Core;
using UnityEngine.Events;
namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        Health target;
        
        [SerializeField] private float speed;
        [SerializeField] GameObject impactFX;
        
        [SerializeField] private float damage;
        [SerializeField] private bool isHoming = false ;
        [SerializeField] UnityEvent onHit;
        [SerializeField] private float lifeAfterImpact = 2f;
        private GameObject instigator = null;
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

        public void SetTarget(Health target, float damage, GameObject instigator)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
        }

        private void OnTriggerEnter(Collider other)
        {
            Health hitHealth = other.GetComponentInParent<Health>();
            if (hitHealth == null || hitHealth != target) return;
            if(target.Dead()) return; 
            onHit.Invoke();  
            if (impactFX != null)
            {
                GameObject impactInstance = Instantiate(impactFX, GetlocationTarget(), Quaternion.identity);
                if (impactInstance.GetComponent<Destroybullet>() == null)
                {
                    impactInstance.AddComponent<Destroybullet>();
                }
            }
            hitHealth.TakeDamage(damage,instigator);
            // Không Destroy ngay, nếu không AudioSource trên projectile sẽ bị tắt tiếng
            speed = 0;
            target = null;
            foreach (Collider c in GetComponentsInChildren<Collider>()) c.enabled = false;
            foreach (Renderer r in GetComponentsInChildren<Renderer>()) r.enabled = false;
            Destroy(gameObject, lifeAfterImpact); 
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
