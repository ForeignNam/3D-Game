using UnityEngine;

namespace RPG.Core
{
public class FollowedCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    void LateUpdate()
    {
        transform.position = target.position;
    }
}
}
