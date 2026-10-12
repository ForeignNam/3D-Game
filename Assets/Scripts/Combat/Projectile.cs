using UnityEngine;
using RPG.Core;
namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        Health target;
        [SerializeField] private float speed;
        [SerializeField] private float damage;

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


        // Update is called once per frame
        void Update()
        {
            if (target != null)
            {
                transform.LookAt(GetlocationTarget());
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
