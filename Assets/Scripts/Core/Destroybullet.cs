using UnityEngine;
namespace RPG.Core
{
    public class Destroybullet : MonoBehaviour
    {
        private ParticleSystem[] particleSystems;

        private void Awake()
        {
            particleSystems = GetComponentsInChildren<ParticleSystem>(true);

            // This component is only useful on a particle-effect instance.
            // Disabling it avoids a MissingComponentException if it is placed
            // on an object which does not contain a Particle System.
            if (particleSystems.Length == 0)
            {
                enabled = false;
            }
        }

        private void Update()
        {
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                if (particleSystem != null && particleSystem.IsAlive(true))
                {
                    return;
                }
            }

            Destroy(gameObject);
        }
    }
}
