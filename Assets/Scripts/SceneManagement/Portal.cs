using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.AI;
namespace RPG.SceneManagement
{
    [DisallowMultipleComponent]
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E, F
        }
         private Fade fade;
        [SerializeField] private int scenetoload = -1;
        [SerializeField] private Transform spawnPoints;
        [SerializeField] private DestinationIdentifier destinationIdentifier;
        [SerializeField] private float fadeOutTime = 3f;
        [SerializeField] private float fadeInTime = 3f;
        [SerializeField] private float fadeWaitTime = 1f;
        private bool isTransitioning;
        private SavingWapper savingWrappe;
             
        private void OnTriggerEnter(Collider other)
        {
            if (!isTransitioning && other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (scenetoload < 0 || scenetoload >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogWarning($"Portal '{name}' does not have a valid scene to load.", this);
                yield break;
            }

            isTransitioning = true;
            DontDestroyOnLoad(gameObject);

            fade = FindFirstObjectByType<Fade>();

            if (fade != null)
            {
                yield return fade.FadeIn(fadeInTime);
            }

            savingWrappe = FindFirstObjectByType<SavingWapper>();
            if (savingWrappe != null)
            {
                savingWrappe.Save();
            }
            else
            {
                Debug.LogWarning("SavingWapper was not found. The scene transition will continue without saving.", this);
            }

            yield return SceneManager.LoadSceneAsync(scenetoload);

            if (savingWrappe != null)
            {
                savingWrappe.Load();
            }

            Portal otherPortal = GetOtherPortal();
            if (otherPortal != null)
            {
                UpdatePlayer(otherPortal);
            }
            else
            {
                Debug.LogWarning($"No destination portal was found after loading scene {scenetoload}.", this);
            }

            yield return new WaitForSeconds(fadeWaitTime);

            if (fade != null)
            {
                yield return fade.FadeOut(fadeOutTime);
            }

            Destroy(gameObject); 
        }
        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("No GameObject tagged 'Player' was found in the destination scene.", this);
                return;
            }

            if (otherPortal.spawnPoints == null)
            {
                Debug.LogWarning($"Destination portal '{otherPortal.name}' has no spawn point assigned.", otherPortal);
                return;
            }

            Vector3 destination = otherPortal.spawnPoints.position;
            if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                destination = hit.position;
            }

            NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(destination);
            }
            else
            {
                player.transform.position = destination;
            }

            player.transform.rotation = otherPortal.spawnPoints.rotation;
        }
        private Portal GetOtherPortal() 
        {
            foreach (Portal portal in FindObjectsByType<Portal>(FindObjectsSortMode.None))
            {
                if (portal == this) continue;
                if (portal.destinationIdentifier != destinationIdentifier) continue;
                return portal;

            }

            return null;
        }



    }
}
