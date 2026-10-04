using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;
namespace RPG.Cinematics {


public class CinematicControllRemover : MonoBehaviour
{

   private GameObject player;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }
        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
         }
        
      void DisableControl(PlayableDirector pd)
        {

         player.GetComponent<ActionSchedule>().CancelCurrentAction();
         player.GetComponent<PlayerController>().enabled = false;

        }
    void EnableControl(PlayableDirector pd)
        {

            player.GetComponent<PlayerController>().enabled = true;

        }
    }
}