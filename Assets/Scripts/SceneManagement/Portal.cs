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

             
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if(scenetoload < 0)
            {
                Debug.Log("Scene to load not set");
                yield break;
            }
            
            DontDestroyOnLoad(gameObject);

            fade = FindObjectOfType<Fade>();

            yield return fade.FadeIn(fadeInTime);
            yield return SceneManager.LoadSceneAsync(scenetoload);
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fade.FadeOut(fadeOutTime);

            Destroy(gameObject); 
        }
        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoints.position);
            player.transform.rotation = otherPortal.spawnPoints.rotation;
          
        }
        private Portal GetOtherPortal() 
        {
            foreach (Portal portal in FindObjectsByType<Portal>(FindObjectsSortMode.None))
            {
                if (portal == this) continue;
                if (portal.destinationIdentifier == destinationIdentifier) continue;
                return portal;

            }

            return null;
        }



    }
}

