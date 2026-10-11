using UnityEngine;

namespace Combat
{
    public class Projectiles : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed;
        

        // Update is called once per frame
        void Update()
        {
            if (target != null)
            {
                transform.LookAt(GetlocationTarget());
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
        private Vector3 GetlocationTarget()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            if (targetCollider == null)
            {
                return target.position;
            }
            return target.position + Vector3.up * targetCollider.height / 2;
        }
        
    }
}