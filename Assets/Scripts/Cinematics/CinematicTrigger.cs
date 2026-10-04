using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicTrigger : MonoBehaviour
    {
        private PlayableDirector director;
        private bool hasPlayed;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasPlayed || !other.CompareTag("Player")) return;

            hasPlayed = true;
            director.Play();
        }
    }
}


